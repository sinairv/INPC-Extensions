# Handy Extensions for INotifyPropertyChanged

This repository contains handy extension methods for classes implementing `INotifyPropertyChanged` (aka INPC). To start add the following to the top of your C# code:

```csharp
using NotifyPropertyChangedExtensions;
```

### How to easily raise INPC in a refactor friendly way

```csharp
public class MyClass : INotifyPropertyChanged
{
    private int _number;

    public int Number
    {
        get { return _number; }
        set 
        { 
            if(_number != value)
            {
                _number = value; 
                this.RaisePropertyChanged(() => Number); 
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
```

### How to easily handle (listen to) INPC in a refactor friendly way

```csharp
MyClass myObject = ...;

myObject.HandleOnPropertyChanged(src => src.Number, sender =>
    {
        Console.WriteLine("Number changed to {0}", sender.Number);
    });
```

### How to do one-way binding from a class that implements INPC

```csharp
var source = new MyClass();
var target = new TargetClass();

source.BindProperty(src => src.Number, target, t => t.Num);

source.Number = 10;
Console.WriteLine(target.Num); // 10
```

You can negate a boolean property while binding it. I use it a lot and find it very handy.

```csharp
source.BindProperty(src => src.IsActive, target, t => !t.IsDeleted);

source.IsActive = false;
Console.WriteLine(target.IsDeleted); // True
```

You can bind primitive data-types in source class to `Nullable` primitive data-types in the target class, and vice versa. 

```csharp
source.BindProperty(src => src.Number, target, t => t.NullableNumber);
```

What happens if source is `null`, and the target is not `Nullable`? 

The default behavior is to update target with default value of that type:

```csharp
target.Number = 10;
source.BindProperty(src => src.NullableNumber, target, t => t.Number);

source.NullableNumber = null;
Console.WriteLine(target.Number); // 0
```

You can also choose not to update target if source becomes `null`:

```csharp
target.Number = 10;
source.BindProperty(src => src.NullableNumber, target, t => t.Number, 
    BindPropertyOptions.DontUpdateWhenSourceIsNullAndTargetIsNotNullable);

source.NullableNumber = null;
Console.WriteLine(target.Number); // still 10
```

## How can I find more information about this library?

At the moment the best way to find out how this library works is by reading the unit tests or at least their titles.

## License: MIT 

Copyright (c) 2014-2015 Sina Iravanian (sina.iravanian@gmail.com)

Permission is hereby granted, free of charge, to any person obtaining a copy of 
this software and associated documentation files (the "Software"), to deal in the 
Software without restriction, including without limitation the rights to use, copy, 
modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
and to permit persons to whom the Software is furnished to do so, subject to the 
following conditions:

The above copyright notice and this permission notice shall be included in all 
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
