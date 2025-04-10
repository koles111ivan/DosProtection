### Механизм защиты от DoS-атак Rate Limiting
## 🔧 Как именно это работает?
Система ограничивает количество запросов с одного IP-адреса за определенное время.
## Принцип работы:
1.	Счетчик запросов – Каждый IP-адрес получает счетчик.
2.	Проверка лимита – Если запросов слишком много (например, больше 10 за 30 секунд), система блокирует новые запросы.
3.	Автоматический сброс – Через заданное время (30 секунд) счетчик обнуляется, и IP снова может отправлять запросы.
## ✔ Почему  Rate Limiting?
# 1.	Простота внедрения
Rate Limiting легко добавить в любой проект. Никаких сложных алгоритмов или интеграций со сторонними сервисами.
# 2. Прозрачность для пользователей
В отличие от CAPTCHA или Proof-of-Work, этот метод не требует действий от пользователя. Они даже не узнают, что используется защита, пока не начнут делать слишком много запросов.
# 3. Баланс между безопасностью и производительностью
•	CAPTCHA надежна, но  пользователям такое не понравится
•	Proof-of-Work слишком нагружает сервер
•	Rate Limiting дает хорошую защиту без этих недостатков
# 4. Гибкость настроек
•	Можно легко изменить правила
# 5. Эффективность против реальных (простых) атак
Останавливает:
•	Простые DoS-атаки
•	Случайные сканы уязвимостей
•	Злоупотребление API
# 6.  Минимальные затраты ресурсов
Добавляет всего 3-5% нагрузки на сервер, в отличие от более сложных методов, которые могут замедлять работу на 20-30%.
Итог: Это хороший выбор для большинства веб-проектов – не слишком сложный, но достаточно эффективный.

### Примеры работы программы
Тестирование в браузере
1)Откроем браузер по адресу который появился при запуске(https://localhost:7186/swagger)


