using Assets.Code._4.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code._2.BUS.NPCs
{
    /// <summary>
    /// Thanh niên nghĩa quân
    /// </summary>
    public class NPC1 : NPCBase
    {
        public override void Start()
        {
            base.Start();
        }
        public override void Initialize()
        {
            Choices = new List<string>();
            Choices.Add(GameSystems.Language["f70adc32fa2c6"]);
            Choices.Add(GameSystems.Language["f70adc32fa2c6"]);

            Content1 = new List<MessageContent>();
            Content1.Add(new MessageContent { Title = "f70adc32fa2c6", Content = "952ae1104f048" });
            Content1.Add(new MessageContent { Title = "f70adc32fa2c6", Content = "c5301ef0eeab3" });
            Content1.Add(new MessageContent { Title = "f70adc32fa2c6", Content = "f9fd7ef19e7ba" });
            Content1.Add(new MessageContent { Title = "f70adc32fa2c6", Content = "c522ca716cb74" });
        }
    }
}
