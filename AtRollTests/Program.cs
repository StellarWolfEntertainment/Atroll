namespace AtRollTests
{
    internal class Program
    {
        private static void Main ()
        {
            AtRoll.Program prog;

            using ( FileStream fs = new ( "rolltest.atroll", FileMode.Open, FileAccess.Read ) )
            {
                prog = AtRoll.Program.Create ( fs );
            }

            for ( int i = 0; i < 10000; i++ )
            {
                Console.WriteLine ( prog.Evaluate () );
            }
        }
    }
}