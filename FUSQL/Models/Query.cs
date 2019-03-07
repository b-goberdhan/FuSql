using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUSQL.Models
{
    public class Query
    {
        public List<Command> Commands = new List<Command>();
        public override string ToString()
        {
            string result = "";
            foreach (var command in Commands)
            {
                result += "Command: \n";
                result += "FIND GROUP " + command.Group.Name + " USING \n";
                foreach (var attribute in command.Group.Attributes)
                {
                    result += attribute.Name + " " + attribute.Operation + " " + attribute.Value + "\n";
                }
            }
            return result;
        }
    }
}
