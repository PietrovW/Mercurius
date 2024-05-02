## Clean Architecture

Clean Architecture to wzorzec architektury, który ma na celu budowanie aplikacji, które są łatwe do utrzymania, skalowania i testowania. Osiąga to poprzez podział aplikacji na różne warstwy o odrębnych odpowiedzialnościach:

* Warstwa domeny (Domain Layer): Stanowi rdzeń aplikacji i zawiera zasady biznesowe oraz encje. Nie powinna mieć zależności zewnętrznych.
* Warstwa aplikacji (Application Layer): Leży tuż za warstwą domeny i działa jako pośrednik między nią a innymi warstwami. Odpowiada za przypadki użycia aplikacji i eksponuje zasady biznesowe z warstwy domeny.
* Warstwa infrastruktury (Infrastructure Layer): Tutaj implementujemy usługi zewnętrzne, takie jak bazy danych, przechowywanie plików, e-maile itp. Warstwa ta
  zawiera implementacje interfejsów zdefiniowanych w warstwie domeny.
* Warstwa prezentacji (Presentation Layer): Odpowiada za interakcje użytkownika i dostarcza danych do interfejsu użytkownika.
* Podstawową zasadą Clean Architecture jest to, że zależności powinny wskazywać z konkretnych warstw na abstrakcyjne warstwy wewnętrzne. Dzięki temu można w przyszłości
 zmieniać konkretne implementacje bez wpływu na inne obszary aplikacji. Dodatkowo, Clean Architecture stosuje strukturalne podejście do organizacji kodu, co ułatwia jego utrzymanie i testowanie1.

Warto również wspomnieć, że Clean Architecture i wzorzec Onion Architecture mają podobne cele i zasady, ale różnią się nieco w szczegółach. Onion Architecture skupia się na warstwie centralnej (zwanej Core), używając interfejsów i odwrócenia zależności, aby odseparować warstwy aplikacji i umożliwić wyższy stopień testowalności12.
Jeśli chcesz zgłębić temat Clean Architecture w C#, polecam zapoznać się z materiałami dostępnymi na stronie Code Maze. Tam znajdziesz więcej informacji oraz przykłady implementacji tego wzorca.

Clean Architecture w kontekście programowania w C# to podejście projektowe, które promuje rozdzielenie kodu na warstwy z wyraźnie określonymi odpowiedzialnościami. Ta architektura została spopularyzowana przez Roberta C. Martina i ma na celu tworzenie systemów, które są łatwiejsze do zarządzania, testowania i utrzymania w dłuższym okresie. Oto podstawowe składniki Clean Architecture w kontekście C#:

### 1. Niezależność od Frameworków

Clean Architecture sugeruje, aby system nie był zależny od używanych bibliotek i frameworków. Oznacza to, że logika biznesowa aplikacji nie powinna być uzależniona od zewnętrznych bibliotek, co ułatwia jej testowanie i modernizację.

### 2. Testowalność
Kod powinien być łatwy do testowania. Logika biznesowa powinna być oddzielona od interfejsu użytkownika, bazy danych, i innych elementów zewnętrznych, co umożliwia łatwe i efektywne testowanie jednostkowe.

### 3. Niezależność UI
Interfejs użytkownika powinien być łatwy do zmiany bez wpływu na resztę systemu. Wzorce takie jak MVVM (Model-View-ViewModel) są często stosowane w aplikacjach C#, aby odseparować logikę biznesową od UI.

### 4. Niezależność od Bazy Danych
Clean Architecture zachęca do projektowania systemu w taki sposób, aby był niezależny od konkretnej bazy danych lub schematu przechowywania danych. Dostęp do danych powinien być realizowany poprzez abstrakcje, co pozwala na łatwą zmianę mechanizmów przechowywania danych.

### 5. Niezależność od Agentów Zewnętrznych
System powinien być zaprojektowany tak, aby integracje z zewnętrznymi agentami, takimi jak API stron trzecich, nie wpływały negatywnie na logikę biznesową.

### Struktura Clean Architecture
1. *Entities (Encje)*: Encje reprezentują zestaw reguł biznesowych, które są fundamentalne dla funkcjonowania aplikacji.
2. *Use Cases (Przypadki Użycia)*: Przypadki użycia orkiestrują przepływ danych do i z encji, implementując reguły niezbędne do wykonania konkretnych funkcji biznesowych.
3. *Interface Adapters (Adaptery Interfejsów)*: Warstwa adapterów konwertuje dane między formatem najbardziej przydatnym dla przypadków użycia i encji, a formatem, który może być używany z bazami danych, API, itp.
4. *Frameworks and Drivers (Frameworki i Sterowniki)*: Ostateczna warstwa, która jest odpowiedzialna za implementację szczegółów zewnętrznych frameworków, GUI, bazy danych itp.

### Implementacja w C#
Podczas implementacji Clean Architecture w C#, używa się często takich podejść jak Dependency Injection (DI) do zarządzania zależnościami między różnymi warstwami. Rozwiązania takie jak .NET Core są idealne do wdrożenia DI i pomagają w utrzymaniu kodu zgodnie z zasadami Clean Architecture. Inversion of Control (IoC) containers również odgrywają kluczową rolę w zapewnieniu elastyczności i niezależności warstw.
Podsumowując, Clean Architecture w C# ma na celu stworzenie systemu, który jest elastyczny, łatwy do testowania i utrzymania, poprzez ścisłe oddzielenie i abstrakcję poszczególnych elementów systemu.

