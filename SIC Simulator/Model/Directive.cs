﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIC_Simulator.Model
{
    internal enum Directive
    {
        END,
		BYTE,
		WORD,
		RESB,
		RESW,
		RESR,
		EXPORTS,
		START
    }
}
