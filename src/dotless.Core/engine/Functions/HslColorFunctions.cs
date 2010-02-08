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

namespace dotless.Core.engine.Functions
{
    public class HueFunction : HslColorFunctionBase
    {
        protected override INode EvalHsl(HslColor color, INode[] args)
        {
            return color.GetHueInDegrees();
        }

        protected override INode EditHsl(HslColor color, Number number)
        {
            color.Hue += number.Value / 360d;
            return color.ToRgbColor();
        }

        protected override string Name
        {
            get { return "hue"; }
        }
    }

    public class SaturationFunction : HslColorFunctionBase
    {
        protected override INode EvalHsl(HslColor color, INode[] args)
        {
            return color.GetSaturation();
        }

        protected override INode EditHsl(HslColor color, Number number)
        {
            color.Saturation += number.Value / 100;
            return color.ToRgbColor();
        }


        protected override string Name
        {
            get { return "saturation"; }
        }
    }

    public class LightnessFunction : HslColorFunctionBase
    {
        protected override INode EvalHsl(HslColor color, INode[] args)
        {
            return color.GetLightness();
        }

        protected override INode EditHsl(HslColor color, Number number)
        {
            color.Lightness += number.Value / 100;
            return color.ToRgbColor();
        }

        protected override string Name
        {
            get { return "lightness"; }
        }
    }
}