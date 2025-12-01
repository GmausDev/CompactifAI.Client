using Xunit;

namespace CompactifAI.Client.Tests;

public class CompactifAIOptionsTests
{
    [Fact]
    public void DefaultOptions_HaveCorrectValues()
    {
        var options = new CompactifAIOptions();

        Assert.Equal("https://api.compactif.ai/v1", options.BaseUrl);
        Assert.Equal(CompactifAIModels.Llama31_8B_Slim, options.DefaultModel);
        Assert.Equal(120, options.TimeoutSeconds);
        Assert.Equal(string.Empty, options.ApiKey);
    }

    [Fact]
    public void SectionName_IsCorrect()
    {
        Assert.Equal("CompactifAI", CompactifAIOptions.SectionName);
    }

    [Fact]
    public void CompactifAIModels_ContainsAllModels()
    {
        Assert.Equal("cai-deepseek-r1-0528-slim", CompactifAIModels.DeepSeekR1_Slim);
        Assert.Equal("cai-llama-4-scout-slim", CompactifAIModels.Llama4Scout_Slim);
        Assert.Equal("llama-4-scout", CompactifAIModels.Llama4Scout);
        Assert.Equal("cai-llama-3-3-70b-slim", CompactifAIModels.Llama33_70B_Slim);
        Assert.Equal("llama-3-3-70b", CompactifAIModels.Llama33_70B);
        Assert.Equal("cai-llama-3-1-8b-slim", CompactifAIModels.Llama31_8B_Slim);
        Assert.Equal("cai-llama-3-1-8b-slim-r", CompactifAIModels.Llama31_8B_Slim_R);
        Assert.Equal("llama-3-1-8b", CompactifAIModels.Llama31_8B);
        Assert.Equal("cai-mistral-small-3-1-slim", CompactifAIModels.MistralSmall31_Slim);
        Assert.Equal("mistral-small-3-1", CompactifAIModels.MistralSmall31);
        Assert.Equal("gpt-oss-20b", CompactifAIModels.GptOss20B);
        Assert.Equal("gpt-oss-120b", CompactifAIModels.GptOss120B);
        Assert.Equal("whisper-large-v3", CompactifAIModels.WhisperLargeV3);
    }
}
