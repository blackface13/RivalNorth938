using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code._2.BUS.NPCs
{
    public interface NPCInterface
    {
        List<string> Choices { get; set; }
        void Initialize();
    }
}
