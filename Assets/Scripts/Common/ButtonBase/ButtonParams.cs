#nullable enable
using UnityEngine;

namespace Common.ButtonBase
{
    public interface IActionParam
    {
    }

    public record SingleActionParam<T>(T Value) : IActionParam;

    public record NoActionParam : IActionParam;

    // public record ToggleActionParams(bool Value) : IActionParams
    // {
    //     public static implicit operator ToggleButtonParams(bool value)
    //     {
    //         return new ToggleButtonParams(value);
    //     }
    // }
    
    public record ColorPickerActionParam(Color Color) : IActionParam
    {
        public static implicit operator ColorPickerActionParam(Color color)
        {
            return new ColorPickerActionParam(color);
        }
    }

    public static class ActionParamUtils
    {
        public static SingleActionParam<T> ToActionParam<T>(this SingleActionParam<T> actionParam) => actionParam;
        public static SingleActionParam<T> ToActionParam<T>(this T t) => new(t);
    }
}