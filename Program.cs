using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainf_ck_Compiler_Engine
{
    internal class Program
    {
        private static void debug()
        {
            // test 1:
            string brainCode = "dbfskdhzggvlfbliff2983876347```+++--->>><<<,,,...-+-+-";
            int testNum = 1;
            Console.WriteLine($"test {testNum}:\n" + "code ->\n" + brainCode + "\n" + BrainFckCompiler.ParseBrainFckCodeToString(brainCode));
            Console.WriteLine("\n");
            
            // test 2:
            brainCode = "+++ppp+++ppp+++nnn----ppp+++nnnjhshd+hhh++"; testNum++;
            Console.WriteLine($"test {testNum}:\n" + "code ->\n" + brainCode + "\n" + BrainFckCompiler.ParseBrainFckCodeToString(brainCode));
            Console.WriteLine("\n");

            // test 3:
            brainCode = "[+++]"; testNum++;
            Console.WriteLine($"test {testNum}:\n" + "code ->\n" + brainCode + "\n" + BrainFckCompiler.ParseBrainFckCodeToString(brainCode));
            Console.WriteLine("\n");

            // test 4:
            brainCode = "+++ppp[+++[---]---]ppp+++"; testNum++;
            Console.WriteLine($"test {testNum}:\n" + "code ->\n" + brainCode + "\n" + BrainFckCompiler.ParseBrainFckCodeToString(brainCode));
            Console.WriteLine("\n");

            // test 5:
            brainCode =
                "[\n" +
                "this is an actual brainfuck script for printing hello world.\n" +
                "]\n" +
                "++++++++++[>+++++++<-]>++. this prints the letter H\n" +
                "<++++++++++[>+++<-]>-. this prints the letter e\n" +
                "+++++++<++[>.<-] this prints \"ll\"\n" +
                ">+++. this prints the letter o\n" +
                ">>+++[<+++++++++++>-]<-. this prints a space character\n" +
                "++++++++>++++[<+++++++++++>-]<+++. this prints the letter W\n" +
                "<. this prints the letter o\n" +
                "+++. this prints the letter r\n" +
                "------. this prints the letter l\n" +
                "--------. this prints the letter d\n" +
                "[-]+++[<+++++++++++>-]<. prints the character !\n";
            testNum++;
            Console.WriteLine($"test {testNum}:\n" + "code ->\n" + brainCode + "\n" + BrainFckCompiler.ParseBrainFckCodeToString(brainCode));
            Console.WriteLine("\n");
        }

        static void Main(string[] args)
        {
            debug();
        }
    }
}
