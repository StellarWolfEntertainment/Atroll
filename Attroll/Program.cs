namespace Atroll
{
    internal class Program
    {
        static void Main ()
        {
            string program =
@"Roll 1d8
Mod 10";

            Executable executable = Executable.Create ( program );
            for ( int i = 0; i < 10; i++ )
            {
                Console.WriteLine ( $"STR: {executable.Evaluate ()}" );
                Console.WriteLine ( $"DEX: {executable.Evaluate ()}" );
                Console.WriteLine ( $"CON: {executable.Evaluate ()}" );
                Console.WriteLine ( $"INT: {executable.Evaluate ()}" );
                Console.WriteLine ( $"WIS: {executable.Evaluate ()}" );
                Console.WriteLine ( $"CHA: {executable.Evaluate ()}" );
                Console.WriteLine ();
            }
        }
    }
}