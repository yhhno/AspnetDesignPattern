using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using System.Web.Caching;

//此类要处理Http上下文缓存 api；

namespace ASPPatterns.Chap2.Service.V2
{
    //ProductV2和ProductRepositoryV2类并不需要任何解释，因为在这里它们只是简单的占位符而已。ProductServiceV2的单个方法（GetAllProudctsInV2）非常简单，它只是协调从缓存中检索商品，一旦缓存为空，就从资源库中检索商品并将结果插入缓存中。
    //当前的代码有什么问题？
    //1.ProductServiceV2依赖于ProductRepositoryV2类，如果ProductRepository类的api发生改变，就需要在ProductServiceV2类中进行修改
    //2.代码不可测试，如果不让真正的ProductRepository类连接真正的数据库，就不能测试ProductServiceV2的方法，因为这两个类存在着紧密的耦合，另一个与测试有关的问题是，该代码依赖于使用Http上下文来缓存商品。很难测试这种与Http上下文紧密耦合的代码、
    //3.被迫使用Http上下文来缓存，在当前状态下，如使用Velocity或Memcached之类的缓存存储提供者，则需要修改ProductServices类以及其他使用缓存的类，Velocity和Memcached都是分布式内存对象缓存系统，可用来替代ASP.NET的默认缓存机制。

    //根据设计原则来重构
    //1问题：首先考虑ProductServiceV2类依赖于ProductRepository类的问题。
    //分析：在当前转态中，ProductServiceV2类非常脆弱，一旦ProductRepository类的API改变，就需要修改ProductService类，这破坏了分离关注点和单一责任原则。
    //解决方法： 采用依赖倒置原则   :依赖抽象而不要依赖具体
    //可以实施依赖倒置原则来解耦ProductServiceV2和ProductRepositoryV2类，让他们都依赖与抽象------一个接口
    //结果：通过引入新接口能够达到什么效果呢？ ProductServiceV2类现在依赖于抽象而不是具体的实现，这意味着ProductServices类完全不知道任何实现，从而确保它不那么容易被破坏，而且代码在整体上对对变化更有弹性



    //2问题：ProductServiceV2类仍然负责具体的实现（构造函数里），而且目前在没有有效地ProductRepository类的情况下，不可能测试代码，依赖注入原则可以帮助解决这个问题
    //分析：ProductServiceV2类仍然与ProductRepositoryV2类的具体实现绑定在一起，因为ProductServicesV2目前的任务就是创建实例，这一点可以从ProductServiceV2类的构造函数中看出。依赖注入（Dependency Injection）可以将创建ProductRepositoryV2实现的责任移到ProductServiceV2类以外，并通过该类的构造器将其注入，
    //解决方法：依赖注入（Dependency Injection）
    //结果：这样就可以在测试期间向ProductServiceV2类传递替代者，从而能够孤立地测试ProudctServiceV2类。通过把获取依赖的责任从ProductServiceV2类中移除，能够确保ProductSeriveV2类遵循单一责任原则：它现在只关心如何协调从缓存或资源库中检索数据，而不是创建具体的IProductRepository实现。

    //依赖注入有3种形式：构造器，方法及属性，我们目前只是用了构造器注入



    //3. 最后一件事就是解决缓存需求对HTTP Context的依赖，为此，我们使用一种简单设计模式提供的服务
    //分析： 由于没有HTTP Context类的源代码，因此不能像为ProductRepositoryV2类所做的那样，简单地为它创建一个接口并让它实现该接口，但是，这个问题在以前已经被解决过无数次了，因此有一个设计模式能够解决这个问题，Adater（适配器）模式主要将一个类的某个接口转换为一个兼容的接口。这样就能够运用该模式，将HTTP Context 缓存API修改成想要使用的兼容的API，然后，可以使用依赖注入原则，通过一个接口将缓存API注入到ProductServiceV2类。
    //解决方法：Adapter模式

    //现在的问题是HTTP Context Cache API 不能隐式实现新的ICacheStorage接口，那么Adapter模式如何帮助解决这个问题？
    //Adapter模式意图： 将一个类的接口转换为客户期望的另一个接口 ;Adapter模式非常简答，它的唯一作用就是让具有不兼容接口的类在一起工作。Adapter模式并不是唯一能够帮助处理缓存数据问题的模式，，稍后，将在第11章的案例研究中，了解Proxy设计模式如何帮助解决缓存问题。
    //结果： 现在可以非常容易地在不影响现有代码的情况先，实现一个新的缓存解决方案，例如：如果希望使用Memcached或MS Velocity，只需创建一个Adapter,让ProductServiceV2类与该缓存存储提供者通过公共的ICacheStorage接口交互即可.


    //4.当前实际中，为了使用使用ProductServiceV2类，总是不得不为构造器提供ICacheStorage实现，但是如果不希望使用缓存数据呢？一个做法及时提供一个null引用，但是这将意味着需要检查空的ICacheStorage实现从而弄乱代码，更好的方法是使用Null Object模式来处理这些特殊情况
    //解决方法： Null Object模式（空对象模式，有时也被称为特殊情况模式）也是一种极为简单的模式。当不希望指定或者不能指定某个类的有效实例而且不希望到处传递null引用时，这个模式就有用武之地，null对象的作用是代替null引用并实现相同的接口，但没有行为。




    /****************结束语：**************/
    //以上简要地展示了如何运用已经介绍的一些模式和原则，首先查看了一段典型的ASP.NET应用程序代码，并演示遵循一些设计原则和设计模式进行重构，在不改变代码行为的情况下提高代码的质量。
    //首先，遵循依赖倒置原则进行重构，把对依赖类的紧密耦合移除。
    //为了进一步提高松散耦合并能够孤立地测试代码，采用依赖注入原则通过ProductServiceV2类的构造器来提供依赖类，
    //然后利用Adapter模式让HTTP Context缓存API实现我们开发的一个缓存接口，
    //最后，介绍了在不希望缓存数据时，如何使用Null Object模式作为“替身”；


    //public class ProductServiceV2
    //{
    //    private ProductRepositoryV2 _productRepositoryV2;

    //    public ProductServiceV2()
    //    {
    //        _productRepositoryV2 = new ProductRepositoryV2();
    //    }

    //    public IList<ProductV2> GetAllProductsInV2(int categoryId)
    //    {
    //        IList<ProductV2> productV2s;
    //        string storageKey=String.Format("productv2s_in_category_id_{0}",categoryId);
    //        productV2s = (List<ProductV2>) HttpContext.Current.Cache.Get(storageKey);
    //        if (productV2s == null)
    //        {
    //            productV2s = _productRepositoryV2.GetAllProductsInV2(categoryId);
    //            HttpContext.Current.Cache.Insert(storageKey, productV2s);
    //        }
    //        return productV2s;
    //    }
    //}



    public class ProductServiceV2
    {
        //v1
        //private ProductRepositoryV2 _productRepositoryV2;

        //v2 修改ProductSeriviceV2 ，以确保它引用的是借口而不是具体实现
        private IProductRepositoryV2 _productRepositoryV2;
       //v3 新增ICacheStorage接口， 可以更新ProductServiceV2类，并使用接口代替HTTP Context实现
        private ICacheStorageV2 _cacheStorageV2;

        //V1
        //public ProductServiceV2()
        //{
        //    _productRepositoryV2 = new ProductRepositoryV2();
        //}


        // //V2 将创建ProductRepositoryV2实现的责任移到ProductServiceV2类以外，并通过该类的构造器将其注入
        //public ProductServiceV2(IProductRepositoryV2 productRepositoryV2)
        //{
        //    _productRepositoryV2 = productRepositoryV2;
        //}


        //v3 新增ICacheStoryV2
        public ProductServiceV2(IProductRepositoryV2 productRepositoryV2, ICacheStorageV2 cacheStorageV2)
        {
            _productRepositoryV2 = productRepositoryV2;
            _cacheStorageV2 = cacheStorageV2;
        }

        public IList<ProductV2> GetAllProductsInV2(int categoryId)
        {
            IList<ProductV2> productV2s;
            string storageKey = String.Format("productv2s_in_category_id_{0}", categoryId);
            ////v1
            //productV2s = (List<ProductV2>)HttpContext.Current.Cache.Get(storageKey);
            //v3 
            productV2s = _cacheStorageV2.Retrieve<List<ProductV2>>(storageKey);
            if (productV2s == null)
            {
                productV2s = _productRepositoryV2.GetAllProductsInV2(categoryId);
                ////v1
                //HttpContext.Current.Cache.Insert(storageKey, productV2s);
                
                //v3
                _cacheStorageV2.Store(storageKey,productV2s);

            }
            return productV2s;
        }
    }
}
