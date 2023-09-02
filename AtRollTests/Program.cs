namespace AtRollTests
{
    internal class Program
    {
        static void Main ( )
        {
            string program =
@"Roll 4d6
While <=2
 Reroll <=2 d6
Drop Lowest";

            AtRoll.Program prog = AtRoll.Program.Create ( program );

            for ( int i = 0; i < 10000; i++ )
            {
                Console.WriteLine ( prog.Evaluate () );
            }
        }
    }
}