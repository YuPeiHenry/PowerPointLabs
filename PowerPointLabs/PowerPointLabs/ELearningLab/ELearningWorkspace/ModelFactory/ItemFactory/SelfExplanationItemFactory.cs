﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.PowerPoint;
using PowerPointLabs.ActionFramework.Common.Log;
using PowerPointLabs.ELearningLab.ELearningWorkspace.Model;
using PowerPointLabs.ELearningLab.Service;
using PowerPointLabs.ELearningLab.Utility;
using PowerPointLabs.Models;
using PowerPointLabs.TextCollection;

namespace PowerPointLabs.ELearningLab.ELearningWorkspace.ModelFactory
{
    public class SelfExplanationItemFactory : AbstractItemFactory
    {
        public SelfExplanationItemFactory(IEnumerable<Effect> effects, PowerPointSlide slide) : base(effects, slide)
        { }
        protected override ClickItem CreateBlock()
        {
            if (effects.Count() == 0)
            {
                return null;
            }

            SelfExplanationClickItem selfExplanation = new SelfExplanationClickItem(captionText: string.Empty);
            foreach (Effect effect in effects)
            {
                string shapeName = effect.Shape.Name;
                string functionMatch = StringUtility.ExtractFunctionFromString(shapeName);
                selfExplanation.tagNo = SelfExplanationTagService.ExtractTagNo(shapeName);
                switch (functionMatch)
                {
                    case ELearningLabText.CaptionIdentifier:
                        selfExplanation.IsCaption = true;
                        break;
                    case ELearningLabText.CalloutIdentifier:
                        selfExplanation.IsCallout = true;
                        break;
                    case ELearningLabText.AudioIdentifier:
                        selfExplanation.IsVoice = true;
                        selfExplanation.VoiceLabel = StringUtility.ExtractVoiceNameFromString(shapeName);
                        if (StringUtility.ExtractDefaultLabelFromVoiceLabel(selfExplanation.VoiceLabel)
                            .Equals(ELearningLabText.DefaultAudioIdentifier))
                        {
                            selfExplanation.VoiceLabel = string.Format(ELearningLabText.AudioDefaultLabelFormat,
                                AudioSettingService.selectedVoice.ToString());
                        }
                        break;
                    default:
                        break;
                }
            }
            return selfExplanation;
        }
    }
}
