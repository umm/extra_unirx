# ExtraUniRx

## What

* Extra Utility for UniRx 

## Requirement

* UniRx
* Your passion for UniRx

## Install

```shell
yarn add "umm-projects/extra_unirx#^1.0.0"
```

## Usage

TestObserver is utility for testing observable.

```
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

SubjectProperty is like a ReactiveProperty, but it's Subject.
It's IObserver & IObservable, and not holding latest value in stream.

```
var property = new SubjectProperty<int>();
property.Subscribe(observer); // behave as IObservable
observable.Subscribe(property); // behave as IObserver
property.Value; // and it's easy to access value
```

## License

Copyright (c) 2018 Takuma Maruyama

Released under the MIT license, see [LICENSE.txt](LICENSE.txt)

