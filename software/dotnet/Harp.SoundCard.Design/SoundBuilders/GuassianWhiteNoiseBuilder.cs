using NWaves.Signals.Builders.Base;
using NWaves.Utils;
using System;
using System.Collections.Generic;
using NWaves.Signals;

namespace Harp.SoundCard.Design.SoundBuilders;

public class GaussianWhiteNoiseBuilder : SignalBuilder
{
    private double _mean;
    private double _stdDev;
    private int? _seed;
    private Random _rand;

    public GaussianWhiteNoiseBuilder()
    {
        ParameterSetters = new Dictionary<string, Action<double>>
        {
            { "mean", param => _mean = param },
            { "stddev, std", param => _stdDev = param },
            {
                "seed", param =>
                {
                    _seed = (int)param;
                    _rand = _seed > 0 ? new Random(_seed.Value) : new Random();
                }
            }
        };
        _mean = 0.0;
        _stdDev = 1.0;
        _rand = new Random();
    }

    // Box-Muller transform for normal distribution
    public override float NextSample()
    {
        var u1 = 1.0 - _rand.NextDouble();
        var u2 = 1.0 - _rand.NextDouble();
        var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        var value = (float)(_mean + _stdDev * randStdNormal);
        
        // Clamp the value to prevent overflow  
        var min = (float)(_mean - _stdDev);
        var max = (float)(_mean + _stdDev);

        return Math.Clamp(value, min, max);
    }

    protected override DiscreteSignal Generate()
    {
        Guard.AgainstNonPositive(_stdDev, "Standard deviation");
        _rand = _seed is > 0 ? new Random(_seed.Value) : new Random();

        return base.Generate();
    }
}
