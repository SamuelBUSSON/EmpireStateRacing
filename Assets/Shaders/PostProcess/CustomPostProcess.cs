using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(ComicsRenderer), PostProcessEvent.BeforeStack, "Custom/Comics")]
public class CustomPostProcess : PostProcessEffectSettings
{
    [Tooltip("Tilling intensity.")]
    public Vector2Parameter tilling = new Vector2Parameter();
    
    [Tooltip("Circle texture")]
    public TextureParameter circleTexture = new TextureParameter();
}

public sealed class ComicsRenderer : PostProcessEffectRenderer<CustomPostProcess>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Comics"));
        sheet.properties.SetVector("_Tilling", settings.tilling);
        sheet.properties.SetTexture("_CircleTexture", settings.circleTexture);
        
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
