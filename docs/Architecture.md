## Clean Architecture
Clean Architecture to wzorzec architektury, który ma na celu budowanie aplikacji, które są łatwe do utrzymania, skalowania i testowania. Osiąga to poprzez podział aplikacji na różne warstwy o odrębnych odpowiedzialnościach:

* Warstwa domeny (Domain Layer): Stanowi rdzeń aplikacji i zawiera zasady biznesowe oraz encje. Nie powinna mieć zależności zewnętrznych.
* Warstwa aplikacji (Application Layer): Leży tuż za warstwą domeny i działa jako pośrednik między nią a innymi warstwami. Odpowiada za przypadki użycia aplikacji i eksponuje zasady biznesowe z warstwy domeny.
* Warstwa infrastruktury (Infrastructure Layer): Tutaj implementujemy usługi zewnętrzne, takie jak bazy danych, przechowywanie plików, e-maile itp. Warstwa ta zawiera implementacje interfejsów zdefiniowanych w warstwie domeny.
* Warstwa prezentacji (Presentation Layer): Odpowiada za interakcje użytkownika i dostarcza danych do interfejsu użytkownika.
* Podstawową zasadą Clean Architecture jest to, że zależności powinny wskazywać z konkretnych warstw na abstrakcyjne warstwy wewnętrzne. Dzięki temu można w przyszłości zmieniać konkretne implementacje bez wpływu na inne obszary aplikacji. Dodatkowo, Clean Architecture stosuje strukturalne podejście do organizacji kodu, co ułatwia jego utrzymanie i testowanie1.

Warto również wspomnieć, że Clean Architecture i wzorzec Onion Architecture mają podobne cele i zasady, ale różnią się nieco w szczegółach. Onion Architecture skupia się na warstwie centralnej (zwanej Core), używając interfejsów i odwrócenia zależności, aby odseparować warstwy aplikacji i umożliwić wyższy stopień testowalności12.

Jeśli chcesz zgłębić temat Clean Architecture w C#, polecam zapoznać się z materiałami dostępnymi na stronie Code Maze. Tam znajdziesz więcej informacji oraz przykłady implementacji tego wzorca.



