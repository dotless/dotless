/* Copyright 2009 dotless project, http://www.dotlesscss.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

using System;

namespace dotless.Core.utils
{
  public class NumberExtensions
  {
    public static decimal Normalize(decimal value)
    {
      return Normalize(value, 0m);
    }
    public static decimal Normalize(decimal value, decimal min)
    {
      return Normalize(value, min, 1m);
    }
    public static decimal Normalize(decimal value, decimal min, decimal max)
    {
      return Math.Min(Math.Max(value, min), max);
    }

    public static double Normalize(double value)
    {
      return Normalize(value, 0d);
    }
    public static double Normalize(double value, double min)
    {
      return Normalize(value, min, 1d);
    }
    public static double Normalize(double value, double min, double max)
    {
      return Math.Min(Math.Max(value, min), max);
    }
  }
}