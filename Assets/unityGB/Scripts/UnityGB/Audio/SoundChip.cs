﻿/*
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

using System.Collections;

namespace UnityGB
{
	public class SoundChip
	{   
		internal SquareWaveGenerator channel1;
		internal SquareWaveGenerator channel2;
		internal VoluntaryWaveGenerator channel3;
		internal NoiseGenerator channel4;
		internal bool soundEnabled = true;
    
		/** If true, channel is enabled */
		internal bool channel1Enable = true, channel2Enable = true,
			channel3Enable = true, channel4Enable = true;
    
		/** Current sampling rate that sound is output at */
		private int sampleRate = 44100;

    
		/** Initialize sound emulation, and allocate sound hardware */
		public SoundChip()
		{
			channel1 = new SquareWaveGenerator(sampleRate);
			channel2 = new SquareWaveGenerator(sampleRate);
			channel3 = new VoluntaryWaveGenerator(sampleRate);
			channel4 = new NoiseGenerator(sampleRate);
		}
    
		/** Change the sample rate of the playback */
		public void SetSampleRate(int sr)
		{
			sampleRate = sr;
        
			channel1.SetSampleRate(sr);
			channel2.SetSampleRate(sr);
			channel3.SetSampleRate(sr);
			channel4.SetSampleRate(sr);
		}
    
		/** Adds a single frame of sound data to the buffer */
		public void OutputSound(IAudioOutput audioOutput)
		{
			if (soundEnabled)
				return;

			int numChannels = 2; // Always stereo for Game Boy
			int numSamples = audioOutput.GetSamplesAvailable();

			byte[] b = new byte[numChannels * numSamples];

			if (channel1Enable)
				channel1.Play(b, numSamples, numChannels);
			if (channel2Enable)
				channel2.Play(b, numSamples, numChannels);
			if (channel3Enable)
				channel3.Play(b, numSamples, numChannels);
			if (channel4Enable)
				channel4.Play(b, numSamples, numChannels);

			audioOutput.Play(b);
		}
	}
}
