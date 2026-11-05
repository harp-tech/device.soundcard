using System;
using System.Collections.Generic;
using NWaves.Signals;
using NWaves.Signals.Builders.Base;
using NWaves.Utils;

namespace Harp.SoundCard.Design.SoundBuilders;

public class UniformWhiteNoiseBuilder : SignalBuilder
{
    private double _min;
    private double _max;
    private int? _seed;
    private Random _rand;

    public UniformWhiteNoiseBuilder()
    {
        ParameterSetters = new Dictionary<string, Action<double>>
        {
            { "min, low, lo", param => _min = param },
            { "max, high, hi", param => _max = param },
            {
                "seed", param =>
                {
                    _seed = (int)param;
                    _rand = _seed > 0 ? new Random(_seed.Value) : new Random();
                }
            }
        };
        _min = -1.0;
        _max = 1.0;
        _rand = new Random();
    }

    public override float NextSample()
    {
        return (float)(_rand.NextDouble() * (_max - _min) + _min);
    }

    protected override DiscreteSignal Generate()
    {
        Guard.AgainstInvalidRange(_min, _max, "Upper amplitude", "Lower amplitude");
        _rand = _seed is > 0 ? new Random(_seed.Value) : new Random();

        return base.Generate();
    }
}
