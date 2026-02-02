namespace AIMS.V2.ExportExcel
{
	public static class ExportExcelConfigurationExtensions
    {
        public static ExportExcelConfiguration<TModel, TGroupKey> Group<TModel, TGroupKey>(
            this ExportExcelConfiguration<TModel> config,
            Func<TModel, TGroupKey> groupBy,
            Func<IGrouping<TGroupKey, TModel>, TModel> groupDataDefinition)
        {
            var newConfig = new ExportExcelConfiguration<TModel, TGroupKey>(config).Group(groupBy, groupDataDefinition);
            return newConfig;
        }
    }
}
