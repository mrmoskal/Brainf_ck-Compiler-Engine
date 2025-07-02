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
            string brainCode = "dbfskdhzggvlfbliff2983876347```+++--->>><<<,,,...-+-+-";
            int testNum = 1;
            Console.WriteLine($"test {testNum}:\n" + "code " + brainCode + "\n" + BrainFckCompiler.ParseBrainFckCodeToString(brainCode));
            Console.WriteLine("\n");

            brainCode = "+++ppp+++ppp+++nnn----ppp+++nnnjhshd+hhh++"; testNum++;
            Console.WriteLine($"test {testNum}:\n" + "code " + brainCode + "\n" + BrainFckCompiler.ParseBrainFckCodeToString(brainCode));
            Console.WriteLine("\n");

            brainCode = "[+++]"; testNum++;
            Console.WriteLine($"test {testNum}:\n" + "code " + brainCode + "\n" + BrainFckCompiler.ParseBrainFckCodeToString(brainCode));
            Console.WriteLine("\n");

            brainCode = "+++ppp[+++[---]---]ppp+++"; testNum++;
            Console.WriteLine($"test {testNum}:\n" + "code " + brainCode + "\n" + BrainFckCompiler.ParseBrainFckCodeToString(brainCode));
            Console.WriteLine("\n");
        }
    }
}
