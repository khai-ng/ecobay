namespace AIMS.V2.ExportExcel
{
    public record GridPosition(int Row, int Col)
    {
        public GridPosition Offset(GridPosition? position)
        => new(Row + (position?.Row ?? 0), Col + (position?.Col ?? 0));
    }
}
