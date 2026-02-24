# AspNet.IdentityJwtRefreshExample

# Описание
Пример реализации аутентификации и авторизации с использованием ASP.NET Core Identity + JWT (Access Token + Refresh Token).

В проекте используется аутентификация через JWT токен с использованием Refresh Token.

* Реализовано:
- [x] Регистрация пользователя
- [x] Логин пользователя
- [x] Генерация Access Token
- [x] Генерация Refresh Token
- [x] Ротация Refresh Token
- [x] Обновление пары токенов (Access + Refresh)
- [x] Logout через revoke Refresh Token
- [x] Авторизация по ролям через [Authorize(Roles = "")]
- [x] Эндпоинт получения Claims текущего пользователя
- [x] Работа JWT в Swagger

Identity используется для:

* создания пользователей
* хеширования пароля
* управления ролями
