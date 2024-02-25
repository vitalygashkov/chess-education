using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLib
{
    public class Chess
    {
        public string fen { get; private set; }
        Board board;
        Moves moves;
        List<FigureMoving> allMoves;

        // "6r1/p1R3pq/5R2/6P1/5PK1/4r3/8/8 w KQkq -"
        // "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
        public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            this.fen = fen;
            board = new Board(fen);
            moves = new Moves(board);
        }

        Chess (Board board)
        {
            this.board = board;
            this.fen = board.fen;
            moves = new Moves(board);
        }

        public Chess Move (string move)
        {
            FigureMoving fm = new FigureMoving(move);
            if (!moves.CanMove(fm))
                return this;
            if (board.IsCheckAfterMove(fm))
                return this;
            Board nextBoard = board.Move(fm);
            Chess nextChess = new Chess(nextBoard);
            return nextChess;
        }

        public char GetFigureAt (int x, int y)
        {
            Square square = new Square(x, y);
            Figure f = board.GetFigureAt(square);
            return f == Figure.none ? '.' : (char)f;
        }

        void FindAllMoves()
        {
            allMoves = new List<FigureMoving>();
            foreach (FigureOnSquare fs in board.YieldFigures())
                foreach (Square to in Square.YieldSquares())
                {
                    FigureMoving fm = new FigureMoving(fs, to);
                    if (moves.CanMove(fm))
                        if (!board.IsCheckAfterMove(fm))
                            allMoves.Add(fm);
                }
        }

        public List<string> GetAllMoves()
        {
            FindAllMoves();
            List<string> list = new List<string>();
            foreach (FigureMoving fm in allMoves)
                list.Add(fm.ToString());
            return list;

        }

        public bool IsCheck()
        {
            return board.IsCheck();
        }

        public bool IsCheckmate()
        {
            if ((GetAllMoves().Count() == 0) && (IsCheck()))
                return true;
            //Moves moves = new Moves(board);
            //Figure fKing = board.moveColor == Color.black ? Figure.blackKing : Figure.whiteKing;
            //foreach (Square square in Square.YieldSquares())
            //{
            //    if (board.GetFigureAt(square) == fKing)
            //    {
            //        FigureOnSquare fosKing = new FigureOnSquare(fKing, square);
            //        foreach (Square square1 in Square.YieldSquares())
            //        {
            //            FigureMoving fm = new FigureMoving(fosKing, square1);
            //            if (moves.CanMove(fm) && !board.IsCheckAfterMove(fm))
            //                return false;
            //        }
            //    }
            //}
            return false;
        }

        public bool IsStalemate()
        {
            if ((GetAllMoves().Count() == 0) && !(IsCheck()))
                return true;
            return false;
        }

        public string GetMoveColor()
        {
            return board.moveColor.ToString();
        }
    }
}
