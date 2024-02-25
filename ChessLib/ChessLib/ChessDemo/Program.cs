using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessLib;

namespace ChessDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            // "6r1/p1R3pq/5R2/6P1/5PK1/4r3/8/8 w KQkq -"
            // "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
            ChessLib.Chess chess = new ChessLib.Chess("rnbqkbn1/ppppp3/7r/6pp/3P1p2/3BP1B1/PPP2PPP/RN1QK1NR w KQq - 0 1");
            List<string> list;
            while (true)
            {
                list = chess.GetAllMoves();
                Console.WriteLine(chess.fen);
                Console.WriteLine(ChessToAscii(chess));
                foreach (string moves in list)
                    Console.Write(moves + "\t");
                Console.WriteLine();
                Console.WriteLine("Ходят: " + chess.GetMoveColor());
                Console.WriteLine("Шах: " + chess.IsCheck());
                Console.WriteLine("Шах и мат: " + chess.IsCheckmate());
                Console.WriteLine();
                Console.Write("> ");
                string move = Console.ReadLine();
                if (move == "q") break;
                if (move == "") move = list[random.Next(list.Count)];
                chess = chess.Move(move);
            }
        }

        static string ChessToAscii (ChessLib.Chess chess)
        {
            string text = " +-----------------+\n";
            for (int y = 7; y >= 0; y--)
            {
                text += y + 1;
                text += " | ";
                for (int x = 0; x < 8; x++)
                    text += chess.GetFigureAt(x, y) + " ";
                text += "|\n";
            }
            text += " +-----------------+\n";
            text += "    a b c d e f g h\n";
            return text;
        }
    }
}
