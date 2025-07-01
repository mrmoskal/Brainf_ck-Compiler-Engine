using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainf_ck_Compiler_Engine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // testing: create the following braif#ck code tokens => ++++++++++[>+++++++<-]>++.++++++++++[>+++<-]>-.
            //Console.WriteLine("test 1:\n=====================================================\n");
            //Console.WriteLine("code:\n\n++++++++++[>+++++++<-]>++.++++++++++[>+++<-]>-.\n");
            //Console.WriteLine("result:\n");
            //TokenNode tokenLstHeader = null; // list header

            //tokenLstHeader = new TokenNode(10, TOKEN_VALUE_TYPE.ADD_NUM_VAL); // first cell -> +10

            //TokenNode tokenList = new TokenNode(0, TOKEN_VALUE_TYPE.LOOP_TOKEN); // start loop (currently: 10 steps):
            //tokenLstHeader.Next = tokenList;

            //TokenNode innerScope = new TokenNode(1, TOKEN_VALUE_TYPE.MOVE_RIGHT_VAL); // in loop -> move right -> 1
            //tokenList.InnerScopeTokenList = innerScope;

            //innerScope.Next = new TokenNode(7, TOKEN_VALUE_TYPE.ADD_NUM_VAL); // in loop -> curr cell -> +7
            //innerScope = innerScope.Next;

            //innerScope.Next = new TokenNode(1, TOKEN_VALUE_TYPE.MOVE_LEFT_VAL); // in loop -> move left -> 1
            //innerScope = innerScope.Next;

            //innerScope.Next = new TokenNode(1, TOKEN_VALUE_TYPE.SUB_NUM_VAL); // in loop -> curr cell -> -1
            //innerScope = innerScope.Next;

            //tokenList.Next = new TokenNode(1, TOKEN_VALUE_TYPE.MOVE_RIGHT_VAL); // after loop -> move right -> 1
            //tokenList = tokenList.Next;

            //tokenList.Next = new TokenNode(2, TOKEN_VALUE_TYPE.ADD_NUM_VAL); // curr cell -> +2
            //tokenList = tokenList.Next;

            //tokenList.Next = new TokenNode(0, TOKEN_VALUE_TYPE.PRINT_TOKEN); // print curr cel
            //tokenList = tokenList.Next;

            //tokenList.Next = new TokenNode(1, TOKEN_VALUE_TYPE.MOVE_LEFT_VAL); // move left -> 1
            //tokenList = tokenList.Next;

            //tokenList.Next = new TokenNode(10, TOKEN_VALUE_TYPE.ADD_NUM_VAL); // curr cell -> +10
            //tokenList = tokenList.Next;

            //tokenList.Next = new TokenNode(0, TOKEN_VALUE_TYPE.LOOP_TOKEN); // start loop (currently: 10 steps)
            //tokenList = tokenList.Next;

            //innerScope = new TokenNode(1, TOKEN_VALUE_TYPE.MOVE_RIGHT_VAL); // in loop -> move right -> 1
            //tokenList.InnerScopeTokenList = innerScope;

            //innerScope.Next = new TokenNode(3, TOKEN_VALUE_TYPE.ADD_NUM_VAL); // in loop -> curr cell -> +10
            //innerScope = innerScope.Next;

            //innerScope.Next = new TokenNode(1, TOKEN_VALUE_TYPE.MOVE_LEFT_VAL); // in loop -> move left -> 1
            //innerScope = innerScope.Next;

            //innerScope.Next = new TokenNode(1, TOKEN_VALUE_TYPE.SUB_NUM_VAL); // in loop -> curr cell -> -1
            //innerScope = innerScope.Next;

            //tokenList.Next = new TokenNode(1, TOKEN_VALUE_TYPE.MOVE_RIGHT_VAL); // after loop -> move right -> 1
            //tokenList = tokenList.Next;

            //tokenList.Next = new TokenNode(1, TOKEN_VALUE_TYPE.SUB_NUM_VAL); // curr cell -> -1
            //tokenList = tokenList.Next;

            //tokenList.Next = new TokenNode(0, TOKEN_VALUE_TYPE.PRINT_TOKEN); // print curr cel
            //tokenList = tokenList.Next;

            //Console.WriteLine(CodeParser.InitTokenParsing(tokenLstHeader));
            //Console.WriteLine("\n=====================================================\n");

            // test 2: compile the above code from writen text directly (the code => ++++++++++[>+++++++<-]>++.++++++++++[>+++<-]>-.)
            //Console.WriteLine("test 2:\n=====================================================\n");
            //Console.WriteLine("code:\n\n++++++++++[>+++++++<-]>++.++++++++++[>+++<-]>-.\n"); 
            //Console.WriteLine("result:\n\n" + BrainFckCompiler.ParseBrainFckCodeToString("++++++++++[>+++++++<-]>++.++++++++++[>+++<-]>-."));
            //Console.WriteLine("\n=====================================================\n");
            string brainCode = "pppppp++++++++++----------<<<<<<<<<<>>>>>>>>>>...,,,ppp---+++---ppp---";
            //Console.WriteLine("test 1:\n" + "code " + brainCode + "\n" + BrainFckCompiler.ParseBrainFckCodeToString(brainCode));
            
            brainCode = "[+++]";
            Console.WriteLine("test 2:\n" + "code " + brainCode + "\n" + BrainFckCompiler.ParseBrainFckCodeToString(brainCode));
        }
    }
}
