/*
 * This file is part of NLua.
 * 
 * Copyright (c) 2014 Vinicius Jarina (viniciusjarina@gmail.com)
 * Copyright (C) 2003-2005 Fabio Mascarenhas de Queiroz.
 * Copyright (C) 2012 Megax <http://megax.yeahunter.hu/>
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using System;

namespace NLua.Event
{
	/// <summary>
	/// Event masks for lua hook callback
	/// </summary>
	/// <remarks>
	/// Do not change any of the values because they must match the lua values
	/// </remarks>
	[Flags]
	public enum EventMasks
	{
		LUA_MASKCALL  = (1 << EventCodes.LUA_HOOKCALL),
		LUA_MASKRET   = (1 << EventCodes.LUA_HOOKRET),
		LUA_MASKLINE  = (1 << EventCodes.LUA_HOOKLINE),
		LUA_MASKCOUNT = (1 << EventCodes.LUA_HOOKCOUNT),
		LUA_MASKALL   = Int32.MaxValue
	}
}