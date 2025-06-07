namespace AuthorizationService.Helpers;

public static class EmailTemplates
{
    public static string GetConfirmEmailBody(string returnUrl, string code) => $@"
<!DOCTYPE html>
<html lang=""ru"">
<head>
  <meta charset=""UTF-8"">
  <title>Подтверждение email — eDice</title>
  <style>
    body {{
      font-family: Arial, sans-serif;
      background-color: #f5f6fa;
      margin: 0;
      padding: 0;
    }}
    .container {{
      max-width: 600px;
      margin: 40px auto;
      background-color: #ffffff;
      border-radius: 8px;
      box-shadow: 0 2px 8px rgba(0,0,0,0.05);
      padding: 20px 40px 40px 40px;
    }}
    .button {{
      display: inline-block;
      padding: 14px 24px;
      background-color: #18181b;
      color: #ffffff !important;
      text-decoration: none;
      border-radius: 6px;
      font-weight: bold;
    }}
    .footer {{
      margin-top: 30px;
      font-size: 12px;
      color: #888888;
      text-align: center;
    }}
  </style>
</head>
<body>
  <div class=""container"">
    <h2>Подтвердите ваш email</h2>
    <p>Здравствуйте!</p>
    <p>Спасибо за регистрацию на платформе <strong>eDice</strong>.</p>
    <p>Пожалуйста, подтвердите ваш адрес электронной почты, нажав на кнопку ниже:</p>

    <p style=""text-align: center; margin: 30px 0;"">
      <a href=""{returnUrl.TrimEnd('/') + '/'}?code={code}"" class=""button"">Подтвердить email</a>
    </p>

    <p>Если вы не регистрировались на eDice, просто проигнорируйте это письмо.</p>

    <div class=""footer"">
      &copy; 2025 eDice. Все права защищены.
    </div>
  </div>
</body>
</html>";

    public static string GetRestorePasswordBody(string returnUrl, string code) => $@"
<!DOCTYPE html>
<html lang=""ru"">
<head>
  <meta charset=""UTF-8"">
  <title>Подтверждение пароля — eDice</title>
  <style>
    body {{
      font-family: Arial, sans-serif;
      background-color: #f5f6fa;
      margin: 0;
      padding: 0;
    }}
    .container {{
      max-width: 600px;
      margin: 40px auto;
      background-color: #ffffff;
      border-radius: 8px;
      box-shadow: 0 2px 8px rgba(0,0,0,0.05);
      padding: 20px 40px 40px 40px;
    }}
    .button {{
      display: inline-block;
      padding: 14px 24px;
      background-color: #18181b;
      color: #ffffff !important;
      text-decoration: none;
      border-radius: 6px;
      font-weight: bold;
    }}
    .footer {{
      margin-top: 30px;
      font-size: 12px;
      color: #888888;
      text-align: center;
    }}
  </style>
</head>
<body>
  <div class=""container"">
    <h2>Подтвердите ваш пароль</h2>
    <p>Здравствуйте!</p>
    <p>Мы получили запрос на подтверждение вашего пароля на платформе <strong>eDice</strong>.</p>
    <p>Пожалуйста, подтвердите ваш пароль, нажав на кнопку ниже:</p>

    <p style=""text-align: center; margin: 30px 0;"">
      <a href=""{returnUrl.TrimEnd('/') + '/'}?code={code}"" class=""button"">Подтвердить пароль</a>
    </p>

    <p>Если вы не запрашивали подтверждение пароля, просто проигнорируйте это письмо.</p>

    <div class=""footer"">
      &copy; 2025 eDice. Все права защищены.
    </div>
  </div>
</body>
</html>";
}