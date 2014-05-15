/*
 * unityGB
 * Copyright (C) 2014 Jonathan Odul (jona@takohi.com)
 * 
 * This file is part of unityGB.
 *
 * unityGB is free software; you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published 
 * by the Free Software Foundation; either version 3 of the License, or
 * (at your option) any later version.
 *
 * unityGB is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using UnityEngine;
using System.Collections;

namespace UnityGB
{
	public abstract class EmulatorBase
	{
		public enum Button
		{
			Up,
			Down,
			Left,
			Right,
			A,
			B,
			Start,
			Select}
		;

		public IVideoOutput Video
		{
			get;
			private set;
		}

		public IAudioOutput Audio
		{
			get;
			private set;
		}

		public ISaveMemory SaveMemory
		{
			get;
			private set;
		}

		public EmulatorBase(IVideoOutput video, IAudioOutput audio = null, ISaveMemory saveMemory = null)
		{
			Video = video;
			Audio = audio;
			SaveMemory = saveMemory;
		}

		public abstract void LoadRom(byte[] data);

		public abstract void RunNextStep();

		public abstract void SetInput(Button button, bool pressed);
	}
}
