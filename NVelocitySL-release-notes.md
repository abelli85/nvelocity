The version includes both .net version and silverlight version. The .net version comes from NVelocity 1.6.1 beta 2 on http://nvelocity.codeplex.com/. The silverlight version is built and named as 'NVelocity 2.0' in this project. Please note that the usage in silverlight is some different. Some work must be done before merging the template:

1. the default nvelocity.properties is recommended to be resource of your silverlight package .xap file. And it's then loaded via:

```C#

    var streamProps = Application.GetResourceStream(new Uri(VM_PROPS, UriKind.Relative)).Stream;
    var extProps = new ExtendedProperties();
    extProps.Load(streamProps, Encoding.UTF8.WebName);
    var ve = new VelocityEngine();
    ve.Init(extProps, false);
```    

2. the template is also recommended to be resource of your
 silverlight package .xap file. And it's loaded via:

```C#

    var res = Application.GetResourceStream(new Uri(VM_LOSS_SUM_LIST, UriKind.Relative));
    var sr = new StreamReader(res.Stream, Encoding.UTF8);
    var body = sr.ReadToEnd();
    StringResourceLoader.GetRepository().PutStringResource("lossVm", body);
```   
 
3. the template is then retrieved in repository which is put in step 2. 
Please note that the output stream is open via SaveFileDialog:

```C#

    var temp = ve.GetTemplate("lossVm");
    var sw = new StreamWriter(dlg.OpenFile());
    temp.Merge(ctx, sw);
    sw.Close();
```      
