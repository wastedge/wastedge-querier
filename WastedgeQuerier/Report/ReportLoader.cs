using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WastedgeQuerier.Report
{
    internal abstract class ReportLoader
    {
        private int _headerRows;
        private int _headerColumns;

        public virtual void Load(ReportDataSet data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            _headerRows = data.Columns.Count + Math.Min(data.Values.Count, 1);
            _headerColumns = data.Rows.Count;

            var dataValues = (JArray)data.Data["values"];

            Resize(
                _headerRows,
                _headerColumns,
                dataValues.Count,
                dataValues.Count == 0 ? 0 : ((JArray)dataValues[0]).Count
            );

            BuildColumns((JArray)data.Data["columns"]);
            BuildRows((JArray)data.Data["rows"]);
            if (dataValues.Count > 0)
                BuildValues(dataValues);
        }

        protected abstract void Resize(int headerRows, int headerColumns, int rows, int columns);

        private void BuildValues(JArray data)
        {
            int rows = data.Count;
            int columns = ((JArray)data[0]).Count;

            for (int row = 0; row < rows; row++)
            {
                var rowData = (JArray)data[row];

                for (int column = 0; column < columns; column++)
                {
                    SetCell(row, column, ((JValue)rowData[column]).Value);
                }
            }
        }

        protected abstract void SetCell(int row, int column, object value);

        private void BuildColumns(JArray data)
        {
            int span = 0;

            for (int i = 0; i < data.Count; i++)
            {
                span += BuildColumn(data[i], 0, _headerColumns + span);
            }
        }

        private int BuildColumn(JToken data, int row, int column)
        {
            if (data.Type != JTokenType.Array)
            {
                SetHeader(row, column, (string)data, 1, 1);
                return 1;
            }

            var subData = (JArray)data;
            int span = 0;

            for (int i = 1; i < subData.Count; i++)
            {
                span += BuildColumn(subData[i], row + 1, column + span);
            }

            if (span == 0)
                span = 1;

            SetHeader(row, column, (string)subData[0], 1, span);

            return span;
        }

        protected abstract void SetHeader(int row, int column, string data, int rowSpan, int columnSpan);

        private void BuildRows(JArray data)
        {
            int span = 0;

            for (int i = 0; i < data.Count; i++)
            {
                span += BuildRow(data[i], _headerRows + span, 0);
            }
        }

        private int BuildRow(JToken data, int row, int column)
        {
            if (data.Type != JTokenType.Array)
            {
                SetHeader(row, column, (string)data, 1, 1);
                return 1;
            }

            var subData = (JArray)data;
            int span = 0;

            for (int i = 1; i < subData.Count; i++)
            {
                span += BuildRow(subData[i], row + span, column + 1);
            }

            if (span == 0)
                span = 1;

            SetHeader(row, column, (string)subData[0], span, 1);

            return span;
        }
    }
}
