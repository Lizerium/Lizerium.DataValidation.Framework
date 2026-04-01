<h1 align="center">🧪 Lizerium Tests 🧪</h1>

<p align="center">
  Набор инженерных тестов, валидаторов и проверок игровых данных<br/>
  для Freelancer-подобных проектов и модификаций в экосистеме <b>Lizerium</b>.
</p>

<p align="center">
  <img src="https://shields.dvurechensky.pro/badge/.NET-8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://shields.dvurechensky.pro/badge/Test%20Layer-Game%20Data%20Validation-1565C0?style=for-the-badge" />
  <img src="https://shields.dvurechensky.pro/badge/Formats-INI%20DLL%20CMP%20MAT%20TXM-37474F?style=for-the-badge" />
  <img src="https://shields.dvurechensky.pro/badge/Scope-Freelancer%20Like%20Projects-2E7D32?style=for-the-badge" />
  <img src="https://shields.dvurechensky.pro/badge/Status-In%20Development-F9A825?style=for-the-badge" />
</p>

---

> [!NOTE]
> Этот проект является частью экосистемы **Lizerium** и относится к направлению:
>
> - [`Lizerium.Frameworks.Structs`](https://github.com/Lizerium/Lizerium.Frameworks.Structs)
>
> Если вы ищете связанные инженерные и вспомогательные инструменты, начните оттуда.

## О проекте

**Lizerium Tests** — это framework автоматизированной инженерной проверки игровых данных, конфигураций, ссылочных ресурсов и структурных зависимостей, используемых в Freelancer-подобных играх и модификациях.

Проект предназначен не для игрового клиента, а для **инженерной валидации контента**:

- `INI`-конфигураций
- `DLL`-ресурсов (`ids_name`, `ids_info`)
- путей к игровым ассетам
- ссылок на эффекты, звуки, модели и контейнеры
- логических зависимостей между игровыми сущностями
- технической целостности модификации в целом

Этот слой помогает заранее находить:

- битые ссылки
- отсутствующие ресурсы
- опечатки в ключах
- некорректные значения
- конфликтующие или неполные игровые записи
- ошибки, которые обычно всплывают уже внутри игры

Проект особенно полезен при:

- сопровождении крупных модификаций
- массовом рефакторинге игровых данных
- миграции старых конфигов
- разработке собственных игровых тулчейнов
- поддержке сложных контентных пайплайнов

> [!TIP]
> Результаты тестов и промежуточные диагностические данные сохраняются в отдельные лог-файлы, что позволяет использовать проект не только как тестовый слой, но и как инструмент технического аудита игровых данных.

## Текущее покрытие проверок

### Общие тесты

- [x] Проверяет являются ли все файлы которые должны быть `BINI` таковыми
- [x] Проверяет наличие анимаций в файлах `CMP`
- [x] Проверяет наличие `Hardpoints` в файлах `CMP`
- [x] Проверка корректности текстур в `STARSPHERE`, `FX`
- [x] Проверяет `use_animation` наличие анимаций в файлах `CMP`
- [x] Проверяет `CMP` на потенциальную возможность анимирования
- [x] Поиск всех `material_library` которые не существуют
- [x] Поиск всех `item_icon` которые не существуют
- [x] Поиск всех `DA_archetype` которые не существуют
- [x] Проверить всех информационных карт на наличие
- [x] Проверить все названия `ids_name` на наличии
- [x] Проверить исправность всех CRC чисел эффектов
- [x] Проверить исправность всех текстур эффектов (ПУТЕЙ К НИМ)
- [x] Проверка всех `explosion_arch`
- [x] Проверка всех `shield_type`
- [x] Проверка всех `debris_type`
- [x] Проверка всех эффектов `[Effect]`
- [x] Проверка всех звуков `[Sound]`
- [x] Проверка всех контейнеров [`LootCrate`] - ящик с добычей, [`CargoPod`] - грузовые отсеки
- [x] Тестирование валидности фракции `[faction =]`

> Задействованы файлы

- `initialworld.ini`
- `ReShade.ini`
- `explosions.ini`
- `weapon_equip.ini`
- `weaponmoddb.ini`
- `engines_ale.ini`
- `effects.ini`
- `sounds.ini`
- `engine_sounds.ini`
- `select_equip.ini`
- `misc_equip.ini`
- `weapon_equip.ini`

> [!CAUTION]
> Вывод происходит в `TESTS_LOGGING/GlobalTests.ini`

### INI → EQUIPMENT

#### CommoditiesPerFactionTests.ini

- [x] - Проверяет исправность введённых типов данных в каждом поле файла
- [x] - Проверяет значения commodity в полях MarketGood в FactionGood

> [!CAUTION]
> Вывод происходит в `TESTS_LOGGING/CommoditiesPerFactionTests.ini`

#### engine_equip.ini

- [x] - Получить все уникальный ключи каждоого блока с данными в файле
- [x] - Получить все типы в файле
- [ ] - Проверить исправность переменных
  - [x] ids_name и ids_info в DLL-ресурсах
  - [x] Проверка ожидаемого количества секций
  - [ ] Проверить, что каждая секция [Engine] успешно преобразуется в объект Engine через TryParse
  - [ ] Проверить, что во всех секциях присутствуют обязательные поля (nickname, ids_name, mass и другие критичные значения)
  - [ ] Проверить, что обязательные строковые поля (например nickname) не пустые и не состоят только из пробелов
  - [ ] Проверить, что в секциях используются только допустимые поля без неизвестных, лишних или опечатанных ключей
  - [ ] Проверить, что все nickname двигателей уникальны и не повторяются между секциями
  - [ ] Проверить, что числовые параметры двигателя (mass, max_force, linear_drag, power_usage и другие) находятся в допустимых диапазонах
  - [ ] Проверить, что значение reverse_fraction находится в допустимом диапазоне и не нарушает физическую логику двигателя
  - [ ] Проверить, что cruise-параметры (cruise_charge_time, cruise_power_usage, cruise_speed) заданы корректно и не содержат аномальных значений
  - [ ] Проверить корректность формата диапазонов значений, таких как character_pitch_range, rumble_atten_range и rumble_pitch_range
  - [ ] Проверить логическую согласованность зависимых полей двигателя (например trail_effect_player без trail_effect, или неполные наборы параметров)
  - [ ] Проверить логическую согласованность звуковых параметров двигателя (например наличие loop-звука при наличии start-звука)
  - [ ] Проверить, что в файле отсутствуют пустые, повреждённые или неполные секции [Engine]
  - [ ] Проверить, что значения volume и power_usage не содержат аномалий, таких как отрицательные или подозрительные значения

> [!CAUTION]
> Вывод происходит в `TESTS_LOGGING/EngineEquipTests.ini`

#### select_equip.ini

- [x] - Получить все уникальные ключи каждого блока с данными в файле
- [x] - Получить все типы в файле
- [x] - Проверить AttachedFX - use_throttle
- [ ] - Проверить Armor
- [ ] - Проверить CargoPod
- [ ] - Проверить Commodity
- [ ] - Проверить InternalFX
- [ ] - Проверить ShieldGenerator
- [ ] - Проверить Shield

> [!CAUTION]
> Вывод происходит в `TESTS_LOGGING/EngineEquipTests.ini`

### FLHook - тесты | генерации

- [ ] - Проверить есть ли в конфигурции регенерации брони все Armor

## Конфигурации

### `app_settings.json`

- Предназначен для того чтобы направить тесты на конкретнуй Freelancer-подобную игру/модификацию, используется в связке с моей [cтруктурной картой игровых данных и форматов классической ветки Lizerium](https://github.com/Lizerium/Lizerium.Game.Structs)

1. Создайте в своей версии проекта файл `app_settings.json` в `Lizerium.Tests/SETTINGS/app_settings.json`
2. Пример содержимого файла, полные пути до папок под анализ заполняете сами [App Settings Example](Lizerium.Tests/Configs/app_settings_example.json)

### `ExpectedBiniFiles.json`

- [Файлы исключения из анализов](Lizerium.Tests/Configs/ExpectedBiniFiles.json)
  - Этот файл нужен чтобы детектировать зашифрованные в `bini` формат файлы, вы сами вписываете список файлов с указанием директории

## Связь с другими направлениями

Данный слой связан с:

- [`Lizerium.Game.Structs`](https://github.com/Lizerium/Lizerium.Game.Structs)
- [`Lizerium.Game.INI`](https://github.com/Lizerium/Lizerium.Game.INI)
- [`Lizerium.Game.Dlls`](https://github.com/Lizerium/Lizerium.Game.Dlls)
- [`Lizerium.Game.SPH`](https://github.com/Lizerium/Lizerium.Game.SPH)

- `Lizerium.Game.CMP` - пока используйте папку с последней модификацией игры (lizup.ru)
- `Lizerium.Game.MAT` - пока используйте папку с последней модификацией игры (lizup.ru)
- `Lizerium.Game.TXM` - пока используйте папку с последней модификацией игры (lizup.ru)
- `Lizerium.Game.3DB` - пока используйте папку с последней модификацией игры (lizup.ru)
- `Lizerium.Game.Music` - пока используйте папку с последней модификацией игры (lizup.ru)

- [`Lizerium.Game.XML.CMP`](https://github.com/Lizerium/Lizerium.Game.XML.CMP)
- [`Lizerium.Game.XML.MAT`](https://github.com/Lizerium/Lizerium.Game.XML.MAT)
- [`Lizerium.Game.XML.TXM`](https://github.com/Lizerium/Lizerium.Game.XML.TXM)
- [`Lizerium.Game.XML.SPH`](https://github.com/Lizerium/Lizerium.Game.XML.SPH)
- [`Lizerium.Game.XML.3DB`](https://github.com/Lizerium/Lizerium.Game.XML.3DB)
- [`Lizerium.Game.XML.ALE`](https://github.com/Lizerium/Lizerium.Game.XML.ALE)
- [`Lizerium.Game.XML.ANM`](https://github.com/Lizerium/Lizerium.Game.XML.ANM)
- [`Lizerium.Game.XML.DFM`](https://github.com/Lizerium/Lizerium.Game.XML.DFM)
- [`Lizerium.Game.XML.UTF`](https://github.com/Lizerium/Lizerium.Game.XML.UTF)
- [`Lizerium.Game.XML.VMS`](https://github.com/Lizerium/Lizerium.Game.XML.VMS)
