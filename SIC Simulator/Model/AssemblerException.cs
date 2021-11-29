using SIC_Simulator.Model;
using System;

// Assigned to Ellis Levine
// Can be tested after Assembler is finished

// Uses inheritance so that, every Exception inherits AssemblerException, which inherits System.Exception class
// More exceptions can be added to this file as development continues

// When calling an exception from another class (ex. AssemblerException ae) doing ae.HResult should return the line number (not tested yet)

namespace SIC_Simulator
{
    internal class AssemblerException : Exception
    {
        public AssemblerException() 
        {

        }

        public AssemblerException(string message) : base(message)
        {

        }

        public AssemblerException(string message, Instruction inst) : base(message)
        { 
            HResult = inst.LineNumber; 
        }
    }

    internal class AssemblerInvalidSymbolException : AssemblerException
    {
        public AssemblerInvalidSymbolException() 
        {

        }

        public AssemblerInvalidSymbolException(string message) : base(message)
        {

        }

        public AssemblerInvalidSymbolException(string message, Instruction inst) : base(message)
        {
            HResult = inst.LineNumber; 
        }
    }

    internal class UndefinedSymbolException : AssemblerException
    {
        public UndefinedSymbolException() 
        {
            
        }
        public UndefinedSymbolException(string message) : base(message)
        {
            
        }
        public UndefinedSymbolException(string message, Instruction inst) : base(message)
        { 
            HResult = inst.LineNumber; 
        }
    }

    internal class InvalidOpcodeException : AssemblerException
    {
        public InvalidOpcodeException() 
        {
        
        }

        public InvalidOpcodeException(string message) : base(message)
        {
        
        }

        public InvalidOpcodeException(string message, Instruction inst) : base(message)
        { 
            HResult = inst.LineNumber; 
        }
    }

    internal class MultipleSymbolDefinitionException : AssemblerException
    {
        public MultipleSymbolDefinitionException() 
        {
            
        }

        public MultipleSymbolDefinitionException(string message) : base(message)
        {
        
        }

        public MultipleSymbolDefinitionException(string message, Instruction inst) : base(message)
        { 
            HResult = inst.LineNumber; 
        }
    }

    internal class InvalidHexConstantException : AssemblerException
    {
        public InvalidHexConstantException() 
        {
        
        }

        public InvalidHexConstantException(string message) : base(message)
        {
            
        }

        public InvalidHexConstantException(string message, Instruction inst) : base(message)
        { 
            HResult = inst.LineNumber; 
        }
    }

    internal class MissingOrExtraOperandException : AssemblerException
    {
        public MissingOrExtraOperandException() 
        {
        
        }

        public MissingOrExtraOperandException(string message) : base(message)
        {
        
        }

        public MissingOrExtraOperandException(string message, Instruction inst) : base(message)
        { 
            HResult = inst.LineNumber; 
        }
    }

    internal class InvalidFileTypeException : AssemblerException
    {
        public InvalidFileTypeException() 
        {
        
        }

        public InvalidFileTypeException(string message) : base(message)
        { 
        
        }
    }

    internal class OutofMemoryException : AssemblerException
    {
        public OutofMemoryException() 
        {
        
        }
        
        public OutofMemoryException(string message) : base(message)
        {
        
        }
        
        public OutofMemoryException(string message, Instruction inst) : base(message)
        {
            HResult = inst.LineNumber; 
        }
    }

    internal class BlankLineException : AssemblerException
    {
        public BlankLineException() 
        {
        
        }
        
        public BlankLineException(string message) : base(message)
        { 
        
        }
        
        public BlankLineException(string message, Instruction inst) : base(message)
        { 
            HResult = inst.LineNumber; 
        }
    }
}
