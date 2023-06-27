namespace GptEngineer.Core.Tests.Infrastructure;

using Base;
using Word2vec.Tools;
using Xunit.Abstractions;

public class Word2VecTests : BaseTest
{
    public Word2VecTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void We_Can_Get_A_Vector_Representation_Of_Dog()
    {
        var path = "F:\\Word2Vec\\glove.42B.300d.txt";
        var vocabulary = new Word2VecTextReader().Read(path);

        var dog = vocabulary.GetRepresentationFor("dog");
        Assert.Equal(200, dog.NumericVector.First());
    }
}