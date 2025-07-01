using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainf_ck_Compiler_Engine
{
    public enum SYNTAX
    {
        NONE,
        ADD,
        SUB,
        MOVE_LEFT,
        MOVE_RIGHT,
        LOOP_START,
        LOOP_END,
        PRINT,
        INSERT
    }

    public enum TOKEN_VALUE_TYPE
    {
        NONE,
        ADD_NUM_VAL,
        SUB_NUM_VAL,
        MOVE_LEFT_VAL,
        MOVE_RIGHT_VAL,
        LOOP_TOKEN,
        PRINT_TOKEN,
        INSERT_TOKEN
    }
}
