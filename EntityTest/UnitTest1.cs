using EntityDataContract;
using EntityDataContract.Validor;
using EntityPresentorProj.Models;
using EntityPresentorProj.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace EntityTest
{
    
    public class MangeEntityPointest 
    {

        MangeEntityPoint mangeEntityPoint = null;
        Mock<ICacheService> cacheService = new Mock<ICacheService>();
        Mock<IDrawPpointService> drawPpointService = new Mock<IDrawPpointService>();
        Mock<IOptions<SignalROptions>> signalr = new Mock<IOptions<SignalROptions>>();
      
        
        [Theory]
        [InlineData("test","test")]
        [InlineData("test1", "test1")]
        public void GetGetCurImageWhenCachEmptyShouldReturnDefaultValue(string curImg,string val)
        {
          
           MangeEntityPoint mangeEntityPoint = new MangeEntityPoint(cacheService.Object, drawPpointService.Object, signalr.Object);
           mangeEntityPoint.SetCurImage(curImg, val);
           var setVaL =   mangeEntityPoint.GetCurImage(curImg).CurImg;

           Assert.True(setVaL== Consts.MainImage);
        }

        [Theory]
        [InlineData("test", "test")]
        [InlineData("test1", "test1")]
        public void GetGetCurImageWhenCacheWasSetShouldReturnSetValue(string curImg, string val)
        {
            cacheService.Setup(a => a.GetValueAsString(It.IsAny<string>())).Returns(val);
            MangeEntityPoint mangeEntityPoint = new MangeEntityPoint(cacheService.Object, drawPpointService.Object, signalr.Object);
            mangeEntityPoint.SetCurImage(curImg, val);
            var setVaL = mangeEntityPoint.GetCurImage(curImg).CurImg;
            Assert.True(setVaL == val);
        }
    }
}