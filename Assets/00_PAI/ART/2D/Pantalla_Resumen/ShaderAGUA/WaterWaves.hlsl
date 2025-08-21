#ifndef WATER_WAVES_INCLUDED
#define WATER_WAVES_INCLUDED

// ---------------------- Funciones auxiliares ----------------------

float2 hash(float2 p)
{
    p = float2(dot(p, float2(127.1, 311.7)),
               dot(p, float2(269.5, 183.3)));
    return -1.0 + 2.0 * frac(sin(p) * 43758.5453123);
}

float noise(float2 p)
{
    const float K1 = 0.366025404; // (sqrt(3)-1)/2
    const float K2 = 0.211324865; // (3-sqrt(3))/6

    float2 i = floor(p + (p.x + p.y) * K1);
    float2 a = p - i + (i.x + i.y) * K2;
    float m = step(a.y, a.x);
    float2 o = float2(m, 1.0 - m);
    float2 b = a - o + K2;
    float2 c = a - 1.0 + 2.0 * K2;

    float3 h = max(0.5 - float3(dot(a, a), dot(b, b), dot(c, c)), 0.0);
    float3 n = h * h * h * h *
               float3(dot(a, hash(i + 0.0)),
                      dot(b, hash(i + o)),
                      dot(c, hash(i + 1.0)));
    return dot(n, float3(70.0, 70.0, 70.0));
}

float4 wave(float2 uv, float4 wave_color, float level, float freq, float amp, float sin_shift, float speed1, float time)
{
    float sinus = sin((uv.x + sin_shift + time * speed1) * freq) * amp;
    float shifted_level = (1.0 + 2.0 * amp) * level - amp; // shift to hide/fill
    float treshold = step(1.0 - sinus - shifted_level, uv.y);
    return wave_color * treshold;
}

// ---------------------- Parámetros de oleaje ----------------------

#define MAX_WAVES 4
#define SUPERPOSITION 4
#define TAU 6.28318
#define PHI 1.618

float height_func(float2 p, float t)
{
    float acc = 0.0;
    for (int i = 0; i < MAX_WAVES; i++)
    {
        for (int j = 0; j < SUPERPOSITION; j++)
        {
            int seed = i + 5 * j;
            float theta = TAU * PHI * 10.0 * float(seed);
            float up = cos(theta) * p.x - sin(theta) * p.y;
            float vp = sin(theta) * p.x + cos(theta) * p.y;
            float initial_phase = TAU * PHI * float(seed);
            float k = pow(2.0, float(i));
            float phase = initial_phase + up * k + cos(vp) + 3.0 * t + 0.5 * k * t;
            float A = cos(phase) / (k * k);
            acc += A;
        }
    }
    return acc;
}

float4 hn_fdm(float2 p, float t)
{
    float h = height_func(p, t);
    float2 vx = float2(0.1, 0.0);
    float2 vy = float2(0.0, 0.1);
    float hx = height_func(p + vx, t);
    float hy = height_func(p + vy, t);
    float dx = (hx - h);
    float dy = (hy - h);

    float3 v1 = normalize(float3(vx.x, 0.0, dx));
    float3 v2 = normalize(float3(0.0, vy.y, dy));
    float3 norm = cross(v1, v2);

    return float4(norm, h);
}

// ---------------------- Función principal ----------------------

void WaterWaves_float(
    float2 uv,
    float time,
    float uv_scale,
    float percentage,
    float speed,
    float wave1_speed,
    float wave2_speed,
    float wave1_freq,
    float wave2_freq,
    float wave1_amp,
    float wave2_amp,
    float3 sun_dir,
    float4 water_color,
    float4 foam_color,
    float4 sky_color,
    float4 specular_color,
    float4 bg_color,
    out float4 outColor
)
{
    float2 uv_screen = (uv - 0.5) * uv_scale;

    // Normales y altura
    float4 nh = hn_fdm(uv_screen * 10.0, time * speed);
    float3 norm = nh.xyz;

    float3 sunN = normalize(sun_dir);

    float4 fragColor;
    if (dot(sunN, norm) > 0.98)
    {
        fragColor = specular_color;
    }
    else
    {
        fragColor = lerp(water_color, sky_color,
                         dot(norm, normalize(float3(0.0, 0.2, 1.0))));
    }

    // Ondas 2D en superficie
    float2 uv2 = float2(uv.y, 1.0 - uv.x);
    float4 shadowWaveColor = fragColor * 0.6;
    shadowWaveColor.a = 1.0;

    float4 wave1c = wave(uv2, shadowWaveColor, percentage, wave1_freq, wave1_amp, 0.0, wave1_speed, time);
    float4 wave2c = wave(uv2, fragColor, percentage, wave2_freq, wave2_amp, 0.7785 * time, wave2_speed, time);

    float4 combined_waves = lerp(wave1c, wave2c, wave2c.a);

    float4 bg = bg_color;
    bg.a = 0.0;

    outColor = lerp(bg, combined_waves, combined_waves.a);
}

#endif