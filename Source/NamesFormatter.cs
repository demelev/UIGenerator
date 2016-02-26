//===----------------------------------------------------------------------===//
//
//  vim: ft=cs tw=80
//
//  Date:    02/25/2016 20:02:18
//  Creator: Emeliov Dmitri <demelev1990@gmail.com>
//
//===----------------------------------------------------------------------===//


namespace UIGenerator
{
    public static class NamesFormatter
    {
        public static string PrivateMember(string input, string type)
        {
            char first = char.ToLower(input[0]);
            return string.Format("_{0}{1}{2}", first, input.Substring(1), type);
        }

        public static string ButtonHandler(string input)
        {
            char first = char.ToUpper(input[0]);
            return string.Format("{0}{1}ButtonClicked", first, input.Substring(1));
        }

        public static string PrefabName(string name)
        {
            if (UIGenerator.s_config.PrefabWithPrefix)
            {
                return "P_" + name;
            }
            else
            {
                return name;
            }
        }

        static public string ObjectName(string name, string type)
        {
            return string.Format("{0}{1}{2}", char.ToUpper(name[0]),
                    name.Substring(1), type);
        }
    }
}
