using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Brainf_ck_Compiler_Engine
{
    public static class CodeParser
    {
        // vars:
        // the main brainf#ck array:
        private const string MAIN_ARR_NAME = "mainArr";
        private const string MAIN_ARR_TYPE = "char"; // in compelation, will always be unsigned (if ever not byte/short/int -> remove the unsigned prefix)
        private const short MAIN_ARR_MAX_SIZE = 256;

        // the main brainf#ck array pointer (offset/index of main array)
        private const string MAIN_ARR_INDEX_NAME = "mainArrOffset";
        private const string MAIN_ARR_INDEX_TYPE = "char"; // in compelation, will always be unsigned (if ever not byte/short/int -> remove the unsigned prefix)

        // for visual clearity:
        private const string EOL = ";\n"; // EOL (end of line).
        private const string SOL = "\t"; // SOL (start of line).

        // functions:
        private static string DuplicateSyntaxStr(string command, int duplicationAmount)
        {
            string result = "";
            for (int offset = 0; offset < duplicationAmount; result += command, offset++) ;
            return result;
        }

        public static string ParseToken(TokenNode token, string SOL_Str = SOL)
        {
            // return a string of the parsed code from the given token.
            if (token == null) return ""; // check token was given.
            if (token.ValType == TOKEN_VALUE_TYPE.LOOP_TOKEN && token.InnerScopeTokenList == null) return ""; // check given token has correct syntax.

            switch (token.ValType)
            {
                // add/subtract value from array cell:
                case TOKEN_VALUE_TYPE.ADD_NUM_VAL:
                case TOKEN_VALUE_TYPE.SUB_NUM_VAL:
                    char operation_arr = token.ValType == TOKEN_VALUE_TYPE.ADD_NUM_VAL ? '+' : '-';
                    return SOL_Str + $"{MAIN_ARR_NAME}[{MAIN_ARR_INDEX_NAME}] {operation_arr}= {token.Value}" + EOL;

                // add/subtract from array index:
                case TOKEN_VALUE_TYPE.MOVE_LEFT_VAL:
                case TOKEN_VALUE_TYPE.MOVE_RIGHT_VAL:
                    char operation_index = token.ValType == TOKEN_VALUE_TYPE.MOVE_RIGHT_VAL ? '+' : '-';
                    return SOL_Str + $"{MAIN_ARR_INDEX_NAME} {operation_index}= {token.Value}" + EOL;

                case TOKEN_VALUE_TYPE.PRINT_TOKEN:
                    string printSyntaxStr = SOL_Str + $"printf(\"%c\", {MAIN_ARR_NAME}[{MAIN_ARR_INDEX_NAME}])" + EOL;
                    return DuplicateSyntaxStr(printSyntaxStr, token.Value);
                
                case TOKEN_VALUE_TYPE.INSERT_TOKEN:
                    string insertSyntaxStr = SOL_Str + $"scanf_s(\"%c\", &{MAIN_ARR_NAME}[{MAIN_ARR_INDEX_NAME}])" + EOL;
                    return DuplicateSyntaxStr(insertSyntaxStr, token.Value);

                // loop over array:
                default:
                    // start loop:
                    string loop = SOL_Str + $"while ({MAIN_ARR_NAME}[{MAIN_ARR_INDEX_NAME}])";
                    
                    // start loop nessting:
                    loop += SOL_Str + "{";
                    loop += "\n";

                    // insert loop innner scope:
                    loop += ParseTokenList(token.InnerScopeTokenList, SOL_Str + SOL);

                    // end loop nessting:
                    loop += SOL_Str + "}";
                    loop += "\n";
                    
                    return loop; // the resulting loop code.
            }
        }
        public static string ParseTokenList(TokenNode tokenList, string SOL_Str = SOL)
        {
            // returns a string cotaining paresed code, made from a given token list.
            string parsedCode = "";    
            
            while (tokenList != null)
            {
                parsedCode += ParseToken(tokenList, SOL_Str);
                tokenList = tokenList.Next;
            }
            
            return parsedCode;
        }
        public static string InitTokenParsing(TokenNode tokenList, string SOL_Str = SOL)
        {
            // returns a string of the complite parsed code from the given tokens.
            string mainInitStr =
                SOL_Str + $"unsigned {MAIN_ARR_TYPE} {MAIN_ARR_NAME}[{MAIN_ARR_MAX_SIZE}] = " + "{0}" + EOL + // init brainf#ck main arr.
                SOL_Str + $"unsigned {MAIN_ARR_INDEX_TYPE} {MAIN_ARR_INDEX_NAME} = 0" + EOL; // init brainf#ck main arr index.

            mainInitStr += ParseTokenList(tokenList, SOL_Str);

            string codeInitStr =
                "#include <stdio.h>" +
                "\n\n" +
                "void main()" +
                "\n" +
                "{" + 
                "\n" +
                mainInitStr +
                "\n" +
                "}";

            return codeInitStr;
        }
    }

    public static class Tokeniser
    {
        // vars:
        private static readonly Dictionary<char, SYNTAX> _syntaxDict = SyntaxDict; // hash char to syntax - for tokenization. 
        private static readonly Dictionary<SYNTAX, TOKEN_VALUE_TYPE> _enumDict = EnumDict; // hash each relevent syntax enum value to token value enum.
        
        private static SYNTAX _currSyntaxFlag = SYNTAX.NONE; // the current active flag.
        private static int _currTokenVal = 0; // the current token value.

        // getters & setters:
        private static Dictionary<char, SYNTAX> SyntaxDict
        { 
            get => new Dictionary<char, SYNTAX> 
            {
                { '+', SYNTAX.ADD },
                { '-', SYNTAX.SUB },
                { '>', SYNTAX.MOVE_RIGHT },
                { '<', SYNTAX.MOVE_LEFT },
                { '[', SYNTAX.LOOP_START },
                { ']', SYNTAX.LOOP_END },
                { '.', SYNTAX.PRINT },
                { ',', SYNTAX.INSERT },
            }; 
        }
        private static Dictionary<SYNTAX, TOKEN_VALUE_TYPE> EnumDict
        { 
            get => new Dictionary<SYNTAX, TOKEN_VALUE_TYPE>
            {
                { SYNTAX.NONE, TOKEN_VALUE_TYPE.NONE },
                { SYNTAX.ADD, TOKEN_VALUE_TYPE.ADD_NUM_VAL },
                { SYNTAX.SUB, TOKEN_VALUE_TYPE.SUB_NUM_VAL },
                { SYNTAX.MOVE_RIGHT, TOKEN_VALUE_TYPE.MOVE_RIGHT_VAL },
                { SYNTAX.MOVE_LEFT, TOKEN_VALUE_TYPE.MOVE_LEFT_VAL },
                { SYNTAX.LOOP_START, TOKEN_VALUE_TYPE.LOOP_TOKEN },
                { SYNTAX.LOOP_END, TOKEN_VALUE_TYPE.NONE },
                { SYNTAX.PRINT, TOKEN_VALUE_TYPE.PRINT_TOKEN },
                { SYNTAX.INSERT, TOKEN_VALUE_TYPE.INSERT_TOKEN },
            }; 
        }
        
        // functions:
        private static SYNTAX GetSyntaxFlagFromChar(char syntaxChar)
        {
            if (!_syntaxDict.ContainsKey(syntaxChar))
                return SYNTAX.NONE;

            return _syntaxDict[syntaxChar];
        }
        private static TokenNode GenerateToken(int tokenVal, TOKEN_VALUE_TYPE tokenValType, TokenNode prevToken = null, TokenNode innerScopeList = null)
        {
            // init new token:
            TokenNode newToken = new TokenNode(
                value: tokenVal,
                valType: tokenValType,
                innerScopeTokenList: innerScopeList
            );

            // return new token:
            if (prevToken != null)  
                prevToken.Next = newToken; // link to token list (if there is one).

            return newToken;
        }
        private static (TokenNode, int, bool) SyntaxToToken(string syntaxStr, int charOffset, SYNTAX syntaxFlag, int tokenVal, TokenNode prevToken = null, TokenNode innerScope = null)
        {
            TOKEN_VALUE_TYPE tokenType = _enumDict[syntaxFlag];
            switch (syntaxFlag)
            {
                case SYNTAX.ADD:
                case SYNTAX.SUB:
                case SYNTAX.MOVE_LEFT:
                case SYNTAX.MOVE_RIGHT:
                case SYNTAX.PRINT:
                case SYNTAX.INSERT:
                    return (
                        GenerateToken(tokenVal: tokenVal, tokenValType: tokenType, prevToken: prevToken),
                        charOffset, 
                        false
                        );

                case SYNTAX.LOOP_END:
                    return (innerScope, charOffset, true);

                case SYNTAX.LOOP_START:
                    TokenNode newToken = GenerateToken(tokenVal: 0, tokenValType: tokenType, prevToken: prevToken);

                    TokenNode newInnerScope = GenerateTokenList(syntaxStr, charOffset + 1);
                    newToken.InnerScopeTokenList = newInnerScope;

                    return (newToken, charOffset, false);

                default:
                    // the NONE syntax flag type:
                    return (null, charOffset, false);
            }
        }
        public static TokenNode GenerateTokenList(string syntaxStr, int offset = 0)
        {
            // returns a tokeniesed list from a given string of tokenable code.
            TokenNode tokenListHead = null, tokenCurrNode = null;
            int initSyntaxFlagOffset = offset > syntaxStr.Length ? syntaxStr.Length - 1 : offset;
            _currSyntaxFlag = GetSyntaxFlagFromChar(syntaxStr[initSyntaxFlagOffset]);

            bool isForceStop;
            SYNTAX nextSyntaxFlag;
            (TokenNode newToken, int updatedOffset, bool isForceStop) result = (null, 0, false);
            for (isForceStop = false; offset < syntaxStr.Length && !isForceStop; offset++)
            {
                nextSyntaxFlag = GetSyntaxFlagFromChar(syntaxStr[offset]); // gset curr syntax flag.

                // increase token val (for compretion sake):
                if (nextSyntaxFlag == _currSyntaxFlag)
                {
                    _currTokenVal++;
                    continue;
                }

                // when syntax flag changes - create the token with the token value (which incremented up till now):
                bool isInnerScope = _currSyntaxFlag == SYNTAX.LOOP_START;
                result = SyntaxToToken(
                    syntaxStr,
                    offset,
                    _currSyntaxFlag,
                    _currTokenVal,
                    !isInnerScope ? tokenCurrNode : null,
                    isInnerScope ? tokenCurrNode : null
                );

                // set & reset variables accordingly:
                isForceStop = result.isForceStop;
                offset = result.updatedOffset;
                _currTokenVal = 1;
                _currSyntaxFlag = nextSyntaxFlag;

                // set current token (add head token if null):
                if (result.newToken == null) continue; // skip null.

                if (tokenListHead == null)
                {
                    // create list head.
                    tokenListHead = result.newToken;
                    continue;
                }

                if (tokenListHead != null && tokenListHead.Next == null)
                {
                    // link 
                    tokenCurrNode = result.newToken;
                    tokenListHead.Next = tokenCurrNode;
                    continue;
                }

                tokenCurrNode.Next = result.newToken;
                tokenCurrNode = tokenCurrNode.Next;
            }

            // generate last token (if there is one left):
            if (_currTokenVal > 0 && !(_currSyntaxFlag == SYNTAX.NONE || _currSyntaxFlag == SYNTAX.LOOP_START))
            {
                result = SyntaxToToken(
                    syntaxStr,
                    offset,
                    _currSyntaxFlag,
                    _currTokenVal,
                    _currSyntaxFlag != SYNTAX.LOOP_START ? tokenCurrNode : null,
                    _currSyntaxFlag == SYNTAX.LOOP_START ? tokenCurrNode : null
                );
            }

            // two possible edge cases (other then just returning a token list):
            if (tokenListHead == null && result.newToken != null)
            {
                // case: no token other then the last token.
                return result.newToken;
            }

            tokenCurrNode.Next = result.newToken; // make sure to add last token (if there is a token).

            // return final token list
            return tokenListHead;
        }
    }

    public static class BrainFckCompiler
    {
        // functions:
        private static TokenNode Tokenies(string brainFckSyntax)
        {
            // tokenies a given str of brainf#ck code.
            // returns a list of tokens, from which the actual code will be compiled
            return Tokeniser.GenerateTokenList(brainFckSyntax);
        }
        private static string ParseTokens(TokenNode tokenList)
        {
            // returns a string of parsed code, made from a given token list
            return CodeParser.InitTokenParsing(tokenList);
        }

        public static string ParseBrainFckCodeToString(string brainFckSyntax)
        {
            // returns a parsed string from a string of brainf#ck code.
            return ParseTokens(Tokenies(brainFckSyntax));
        }
    }
}
