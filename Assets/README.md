# ExtraUniRx

## What

* Extra Utility for UniRx

## Requirement

* UniRx
* Your passion for UniRx

## Install

### With Unity Package Manager

```bash
upm add package dev.upm-packages.extra_unirx
```

Note: `upm` command is provided by [this repository](https://github.com/upm-packages/upm-cli).

You can also edit `Packages/manifest.json` directly.

```jsonc
{
  "dependencies": {
    // (snip)
    "dev.upm-packages.extra_unirx": "[latest version]",
    // (snip)
  },
  "scopedRegistries": [
    {
      "name": "Unofficial Unity Package Manager Registry",
      "url": "https://upm-packages.dev",
      "scopes": [
        "dev.upm-packages"
      ]
    }
  ]
}
```

### Any other else (classical umm style)


```shell
yarn add "umm/extra_unirx#^1.0.0"
```

## Usage

### TestObserver

TestObserver is utility for testing observable.

```csharp
var subject = new Subject<int>();
var observer = new TestObserver<int>();

subject.Subscribe(observer);

subject.OnNext(1);
subject.OnNext(2);
subject.OnCompleted();

Assert.AreEqual(2, observer.OnNextCount);
Assert.AreEqual(1, observer.OnNextValues[0]);
Assert.AreEqual(2, observer.OnNextValues[1]);
Assert.AreEqual(1, observer.OnCompletedCount);
```

### SubjectProperty

SubjectProperty is like a ReactiveProperty, but it's Subject.
It's IObserver & IObservable, and not holding latest value in stream.

```csharp
var property = new SubjectProperty<int>();
property.Subscribe(observer); // behave as IObservable
observable.Subscribe(property); // behave as IObserver
property.Value; // and it's easy to access value
```

### IsDefault(), IsNotDefault()

IsDefault(), IsNotDefault() are operators for `IObservable<T>`.
These operators stream values when the streamed value is default (or not default).

```csharp
var intSubject = new Subject<int>();
intSubject.IsDefault().Subscribe(x => Debug.Log(x)); // 0
intSubject.IsNotDefault().Subscribe(x => Debug.Log(x)); // 100
intSubject.OnNext(0);
intSubject.OnNext(100);

var objectSubject = new Subject<AnyClass>();
objectSubject.IsDefault().Subscribe(x => Debug.Log(x)); // "null"
objectSubject.IsNotDefault().Subscribe(x => Debug.Log(x)); // "AnyClass"
objectSubject.OnNext(null);
objectSubject.OnNext(new AnyClass());
```

Both methods accept a selector as an argument to select values to determine.

```csharp
var intSubject = new Subject<int>();
intSubject.IsDefault(x => x - 100).Subscribe(x => Debug.Log(x)); // 0
intSubject.IsDefault(x => x - 100).Subscribe(x => Debug.Log(x)); // 0, 100
intSubject.IsNotDefault(x => x - 100).Subscribe(x => Debug.Log(x)); // 100
intSubject.OnNext(0);
intSubject.OnNext(100);

class AnyClass { bool Value; }

var objectSubject = new Subject<AnyClass>();
objectSubject.IsDefault().Subscribe(x => Debug.Log(x)); // null, "AnyClass"
objectSubject.IsDefault().Subscribe(x => Debug.Log(x.Value)); // throw NullReferenceException in Subscribe()
objectSubject.IsNotDefault().Subscribe(x => Debug.Log(x.Value)); // true
objectSubject.OnNext(null);
objectSubject.OnNext(new AnyClass() { Value = false });
objectSubject.OnNext(new AnyClass() { Value = true });
```

## License

Copyright (c) 2018 Takuma Maruyama

Released under the MIT license, see [LICENSE.txt](LICENSE.txt)
