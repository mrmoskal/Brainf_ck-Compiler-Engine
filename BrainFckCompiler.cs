﻿using System;
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
        private static int _syntaxOffset = 0; // the offset of the current syntax element in syntax string.
        
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
        private static TokenNode GenerateToken(int tokenVal, TOKEN_VALUE_TYPE tokenValType, TokenNode innerScopeList = null)
        {
            // init new token:
            TokenNode newToken = new TokenNode(
                value: tokenVal,
                valType: tokenValType,
                innerScopeTokenList: innerScopeList
            );

            return newToken;
        }
        private static TokenNode SyntaxToToken(string syntaxStr, TokenNode prevToken = null)
        {
            // create token based of current syntax flag.
            switch (_currSyntaxFlag)
            {
                case SYNTAX.ADD:
                case SYNTAX.SUB:
                case SYNTAX.MOVE_RIGHT:
                case SYNTAX.MOVE_LEFT:
                case SYNTAX.PRINT:
                case SYNTAX.INSERT:
                    return GenerateToken(
                        tokenVal: 1,
                        tokenValType: _enumDict[_currSyntaxFlag]
                    );

                case SYNTAX.LOOP_START:
                    // increase offset to next part of syntax string:
                    _syntaxOffset++;

                    // generate inner scope:
                    TokenNode innerScopeList = GenerateTokenList(syntaxStr);

                    // return new loop token with inner scope:
                    return GenerateToken(
                        tokenVal: 0,
                        tokenValType: _enumDict[SYNTAX.LOOP_START],
                        innerScopeList: innerScopeList
                    );

                default: // SYNTAX.NONE / SYNTAX.LOOP_END
                    return null;
            }
        }
        public static TokenNode GenerateTokenList(string syntaxStr, bool isInnerScopeList = true)
        {
            // generate a list of tokens from given syntax string.
            TokenNode tokenListHead = null, tokenCurrNode = null;
            _syntaxOffset = isInnerScopeList ? _syntaxOffset : 0; // init syntax offset (only at parse start).
            _currSyntaxFlag = GetSyntaxFlagFromChar(syntaxStr[_syntaxOffset]); // set current syntax flag.

            // init offset at parse start.
            // also, parse all none tokenable parts of syntax string from current start to next tokenable syntax.
            int tempOffset; // to check offset change later.
            for (tempOffset = _syntaxOffset; _syntaxOffset < syntaxStr.Length && _currSyntaxFlag == SYNTAX.NONE; _syntaxOffset++)
            {
                _currSyntaxFlag = GetSyntaxFlagFromChar(syntaxStr[_syntaxOffset]);
            }

            bool isInnerScopeEnd = _currSyntaxFlag == SYNTAX.LOOP_END && isInnerScopeList; // case of end in inner scope.
            if (_currSyntaxFlag == SYNTAX.NONE || isInnerScopeEnd) return null; // no parsable syntax found (possibly inside loop scope).

            // set token list head:
            tokenListHead = SyntaxToToken(syntaxStr);
            _syntaxOffset += _syntaxOffset == tempOffset ? 1: 0; // after first token created - move to next syntax element in string (if not moved already).

            // set current node & start parse loop:
            SYNTAX currSyntaxFlagTemp;
            for (tokenCurrNode = tokenListHead; _syntaxOffset < syntaxStr.Length && tokenCurrNode != null; _syntaxOffset++)
            {
                currSyntaxFlagTemp = GetSyntaxFlagFromChar(syntaxStr[_syntaxOffset]); // get current syntax flag.

                // skip none tokenable syntax:
                if (currSyntaxFlagTemp == SYNTAX.NONE)
                    continue;
                // increase token value if flag is same:
                if (currSyntaxFlagTemp == _currSyntaxFlag)
                {
                    tokenCurrNode.Value++;
                    continue;
                }

                _currSyntaxFlag = currSyntaxFlagTemp; // flag changed.
                tokenCurrNode.Next = SyntaxToToken(syntaxStr); // get new token

                // compress tokens if possible
                bool isNoneCompressableToken = tokenCurrNode.Next == null || tokenCurrNode.Next.ValType == TOKEN_VALUE_TYPE.LOOP_TOKEN;
                if (!isNoneCompressableToken && tokenCurrNode.ValType == tokenCurrNode.Next.ValType)
                {
                    tokenCurrNode.Value += tokenCurrNode.Next.Value;
                    tokenCurrNode.Next = null;
                    continue;
                }

                tokenCurrNode = tokenCurrNode.Next; // change current token.
            }

            // return generated token list:
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
            return Tokeniser.GenerateTokenList(brainFckSyntax, isInnerScopeList: false);
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
