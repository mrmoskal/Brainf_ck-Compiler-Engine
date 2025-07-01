using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Brainf_ck_Compiler_Engine
{
    public class TokenNode
    {
        // vars:
        private int _value;
        private readonly TOKEN_VALUE_TYPE _valType;

        private TokenNode _next;
        private TokenNode _innerScopeTokenList;

        // setters & getters:
        public int Value { get => _value; set => _value = value; }
        public TOKEN_VALUE_TYPE ValType { get => _valType; }

        public TokenNode Next { get => _next; set => _next = value; }
        public TokenNode InnerScopeTokenList { get => _innerScopeTokenList; set => _innerScopeTokenList = value; }

        
        // constructors:
        public TokenNode(int value, TOKEN_VALUE_TYPE valType, TokenNode next = null, TokenNode innerScopeTokenList = null)
        {
            this._value = value;
            this._valType = valType;
            this._next = next;
            this._innerScopeTokenList = innerScopeTokenList;
        }
    }
}
