<br />
<p align="center">
  <img src="https://github.com/user-attachments/assets/b4c8f2ae-a62a-4192-a88a-5b28e114bb63" alt="Maybe-Logo" width="40%" />
</p>


[![Discord Shield](https://img.shields.io/discord/831165782750789672)](https://discord.com/invite/xPHTuHCmuV)
[![Open in Dev Containers](https://img.shields.io/static/v1?label=Dev%20Containers&message=Open&color=blue&logo=visualstudiocode)](https://vscode.dev/redirect?url=vscode://ms-vscode-remote.remote-containers/cloneInVolume?url=https://github.com/OpenBB-finance/OpenBB)
<a href="https://codespaces.new/OpenBB-finance/OpenBB">
  <img src="https://github.com/codespaces/badge.svg" height="20" />
</a>
<a target="_blank" href="https://colab.research.google.com/github/OpenBB-finance/OpenBB/blob/develop/examples/googleColab.ipynb">
  <img src="https://colab.research.google.com/assets/colab-badge.svg" alt="Open In Colab"/>
</a>
[![PyPI](https://img.shields.io/pypi/v/openbb?color=blue&label=PyPI%20Package)](https://pypi.org/project/openbb/)


## Why Maybe Plugins?
Maybe Plugins let you **build once, use anywhere** across backtests, paper trading, and live markets.


![imagde](https://private-user-images.githubusercontent.com/35243/438235619-13fc5ef4-ce0f-4073-a163-9dbc3eb4c8e5.png?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NDk5MzE2NjcsIm5iZiI6MTc0OTkzMTM2NywicGF0aCI6Ii8zNTI0My80MzgyMzU2MTktMTNmYzVlZjQtY2UwZi00MDczLWExNjMtOWRiYzNlYjRjOGU1LnBuZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNTA2MTQlMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjUwNjE0VDIwMDI0N1omWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPTAwMmZlOGI0MzU2ODM2YjQ2YzA0NTkzYTc2YzhhMzY2ZWNkYmY5MjM3NWViYjQ2MTljYTUxNzMyZDcwNTZlNTcmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0In0.3eVg2xATusT5XfRT6t5CI2u41xWZ_scRlcB7pMK3RnI)


They are:
- 🔌 Modular & reusable components
- 📦 Environment-agnostic (backtest, sandbox, live)
- 🧩 Easy to plug into any strategy or workflow
- 🌐 Exchange-agnostic with unified interfaces

Create, share, or combine plugins for indicators, strategies, risk controls, and more — all while keeping your code clean and scalable.

<summary><h2> What Can I Do with Maybe?</h2></summary>

Maybe offers a modular, open-source payments infrastructure designed for flexibility and control. Apart from our Payment Suite offering, this solution allows businesses to pick and integrate only the modules they need on top of their existing payment stack — without unnecessary complexity or vendor lock-in.

Each module is independent and purpose-built to optimize different aspects of payment processing.

### Trade Stocks, Crypto, Futures, and Forex

```python
import Maybe
"""
This example shows how backtest over tweets
"""

class TwitterBot(Maybe.Model):
    def main(self, args):
        while self.has_data:
            self.backtester.value_account()
            self.sleep('1h')

    def event(self, type_: str, data: str):
        # Now check if it's a tweet about Tesla
        if 'tsla' in data.lower() or 'gme' in data.lower():
            # Buy, sell or evaluate your portfolio
            pass


if __name__ == "__main__":
    exchange = Maybe.Alpaca()
    model = TwitterBot(exchange)

    # Add the tweets json here
    model.backtester.add_custom_events(Maybe.data.JsonEventReader('./tweets.json'))
    # Now add some underlying prices at 1 month
    model.backtester.add_prices('TSLA', '1h', start_date='3/20/22', stop_date='4/15/22')

    # Backtest or run live
    print(model.backtest(args=None, initial_values={'USD': 10000}))

```

## 🛠️ Installation

Follow the steps below to install and run this project on your local machine.

### 1. 📦 Prerequisites

Make sure you have the following tools installed on your system:

- [Node.js](https://nodejs.org/)  (v14.x or newer)
- [npm](https://www.npmjs.com/)  (comes with Node.js)
- [Git](https://git-scm.com/) 
- Python 3.x (optional, if any indicators or tools require it)

> You can also use Docker to run the project in an isolated environment.

### 3. 📦 Install Dependencies

Install the required npm packages:

```bash
npm install
```

If the plugin uses Python-based tools or indicators, install the Python dependencies as well:

```bash
pip install -r requirements.txt
```

---

### 4. 🔐 Set Up Exchange API Keys

To allow the plugin to interact with cryptocurrency exchanges, set up your API credentials.

Create a `.env` file in the root directory and add your exchange keys:

```env
EXCHANGE_API_KEY=your_api_key_here
EXCHANGE_SECRET_KEY=your_secret_key_here
```

> Make sure not to commit this file to version control. It should remain private.

---

### 5. ⚙️ Configure the Plugin

Each plugin comes with a configuration file. For example:

```yaml
# config/rsi-config.yaml
strategy:
  rsi_period: 14
  overbought_threshold: 70
  oversold_threshold: 30
  symbol: BTC/USDT
  interval: "1h"
```

You can modify these values based on your trading preferences.

---

### 6. ▶️ Run the Plugin

Once everything is set up, start the plugin:

```bash
npm start -- --plugin rsi --config config/rsi-config.yaml
```

Or, if you're using a custom script:

```bash
node index.js --plugin rsi --config config/rsi-config.yaml
```

---

### 7. 🐳 Optional: Run with Docker

Build and run the plugin using Docker:

```bash
docker build -t Maybe-rsi-plugin .
docker run -it --env-file .env Maybe-rsi-plugin
```

```mermaid
erDiagram
    PLUGIN {
        string id
        string name
        string type
    }

    PLUGIN ||--o{ STRATEGY : implements
    STRATEGY ||--o{ INDICATOR : uses
    STRATEGY ||--o{ EXECUTOR : runs
    EXECUTOR ||--o{ MARKET_INTERFACE : interacts
    MARKET_INTERFACE }|--o{ EXCHANGE : connects
    PLUGIN ||--o{ CONFIGURATION : requires
    PLUGIN ||--o{ LOGGING : logs
```



<p align="center">
    <img src="https://minkxx-spotify-readme.vercel.app/api?theme=dark&rainbow=true&scan=true&spin=True" alt="Preview">
</p>

<p align="center">
  <img src="https://github.com/tarikmanoar/tarikmanoar/raw/output/github-snake-dark.svg" alt="snake"></center>
</p>

## Contributors

 wouldn't be  without you. If we are going to disrupt financial industry, every contribution counts. Thank you for being part of this journey.

<a href="https://github.com/OpenBB-finance/OpenBB/graphs/contributors">
   <img src="https://contributors-img.web.app/image?repo=OpenBB-finance/OpenBB" width="800"/>
</a>
