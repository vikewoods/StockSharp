﻿namespace StockSharp.Algo.Indicators
{
	using System.ComponentModel;

	using StockSharp.Algo.Candles;
	using StockSharp.Localization;

	/// <summary>
	/// Volume weighted moving average.
	/// </summary>
	/// <remarks>
	/// http://www2.wealth-lab.com/WL5Wiki/VMA.ashx http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:vwap_intraday.
	/// </remarks>
	[DisplayName("VMA")]
	[DescriptionLoc(LocalizedStrings.Str823Key)]
	public class VolumeWeightedMovingAverage : LengthIndicator<decimal>
	{
		// Текущее значение числителя
		private readonly Sum _nominator = new Sum();

		// Текущее значение знаменателя
		private readonly Sum _denominator = new Sum();

		/// <summary>
		/// To create the indicator <see cref="VolumeWeightedMovingAverage"/>.
		/// </summary>
		public VolumeWeightedMovingAverage()
		{
			Length = 32;
		}

		/// <summary>
		/// To reset the indicator status to initial. The method is called each time when initial settings are changed (for example, the length of period).
		/// </summary>
		public override void Reset()
		{
			base.Reset();
			_denominator.Length = _nominator.Length = Length;
		}

		/// <summary>
		/// Whether the indicator is set.
		/// </summary>
		public override bool IsFormed => _nominator.IsFormed && _denominator.IsFormed;

		/// <summary>
		/// To handle the input value.
		/// </summary>
		/// <param name="input">The input value.</param>
		/// <returns>The resulting value.</returns>
		protected override IIndicatorValue OnProcess(IIndicatorValue input)
		{
			var candle = input.GetValue<Candle>();

			var shValue = _nominator.Process(input.SetValue(this, candle.ClosePrice * candle.TotalVolume)).GetValue<decimal>();
			var znValue = _denominator.Process(input.SetValue(this, candle.TotalVolume)).GetValue<decimal>();

			return znValue != 0 
				? new DecimalIndicatorValue(this, (shValue / znValue)) 
				: new DecimalIndicatorValue(this);
		}
	}
}