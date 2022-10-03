# Tron
### В игре 3 слоя:

- **Джо** отвечает за слой хранения данных (Data). Нужно реализовать создание файла в который будут записываться рекорды в столбик. придумать как доставать нужный рекорд + уникальные функции (например, кто из игроков за последние 10 раз набрал большее количество побед (красный или синий)). Дать доступ классов Жене.
#### Задачи: 
1) Создание файла для записи рекордов с корневой папке (в той папке где храниться игра)
2) Записть нового рекорда в новую строку. Записать кто выйграл (зеленый или красный) в формате G и R
3) Удаление файла с рекордами / Очистка файла с рекордами
4) Чтение из файла чтобы показать последние 10 рекордов
***

- **Женя** отвечает за слой бизнес логики или просло логики (Buisness logic). Необхожимо получим от Джо и Саши их реализации начать создавать обработку событий в игре. Например, что происходит после того как один пользователь врезается в хвост другого. Реализовывается в последнюю очередь.
#### Задачи: 
1) Отрисовка движения игроков
2) Обработка столкновения персонажей
***

- **Саша** отвечает за слой взаимодействия с пользователем (Client Server). Необходимо будет создать класс для работы с GUI интерфейсом + добавить классы констант для клавиатуры, мышки. Дать доступы к классам Жене.
#### Задачи: 
1) Создание класса для взаимодействия с полльзователем (обработка нажатий мыши и клавиатуры)
2) Создвние класса стартового интерфейса
3) Создание экрана игры
4) Создание отрисовки 1 персонажа (чтобы потом Женя написал передвижение по экрану)
5) Создание экрана для результатов
6) Создание экрана для самой игры
