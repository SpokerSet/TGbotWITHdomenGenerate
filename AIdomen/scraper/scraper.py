from bs4 import BeautifulSoup
import urllib3
import ssl
import pandas
import tldextract
from concurrent.futures import ThreadPoolExecutor
import requests

session = requests.Session()
adapter = requests.adapters.HTTPAdapter(pool_connections=100, pool_maxsize=50)
session.mount('http://', adapter)

ssl._create_default_https_context = ssl._create_unverified_context

# Load data into pandas dataframe
df = pandas.read_csv('D:\AIdomen\scraper\data\majestic_million.csv')

# We will fetch and try to get the meta

def fetch_meta(url, session):
    try:
        res = session.get(url, headers=headers, timeout=5)
        soup = BeautifulSoup(res.content, 'html.parser')
        description = soup.find(attrs={'name': 'Description'})

        # If name description is big letters:
        if description is None:
            description = soup.find(attrs={'name': 'description'})

            if description is None:
                print('Context is empty, pass')
                meta_data = None
            else:
                content = description.get('content','')
                url_clean = tldextract.extract(url)
                suffix = url_clean.suffix
                domain = url_clean.domain

                if suffix is None:
                    print('Domain suffix is None ' + str(url))
                    meta_data = None
                    
                # Domains with weird tld's are not in our priority. We would like to keep our training data as clean as possible.
                else:
                    print(url)
                    print(url_clean)
                    print(content)
                    meta_data = (str(content) + ' = @ = ' + str(domain) + '.' + str(suffix) + '\n')
                return meta_data
    except requests.exceptions.RequestException as e:
        print(e)
        return None

headers = {'User-Agent': 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.47 Safari/537.36','Accept-Language':'en-US'}
req = urllib3.PoolManager(maxsize=50)

domain_amount = 10000
increment = int(domain_amount / 5)

meta_contexts = [
    open(f"D:\AIdomen\scraper\data\meta_context_{i}.csv", "w", encoding="utf-8")
    for i in range(1, 6)
]

# Функция для обработки домена
def process_domain(domain):
    url = f"https://{domain}"
    meta_data = fetch_meta(url, session)
    if meta_data is not None:
        return meta_data
    return ""

# Создаем пул потоков
with ThreadPoolExecutor(max_workers=50) as executor:
    # Создаем список доменов для обработки
    domains = df["Domain"].tolist()
    # Разбиваем список на 5 частей
    domain_chunks = [
        domains[i:i + increment] for i in range(0, domain_amount, increment)
    ]
    # Обрабатываем каждую часть в отдельном потоке
    for i, chunk in enumerate(domain_chunks):
        # Передаем каждый домен из части в поток для обработки
        results = executor.map(process_domain, chunk)
        # Записываем результаты в файл
        for result in results:
            meta_contexts[i].write(result)

# Закрываем файлы
for meta_context in meta_contexts:
    meta_context.close()
    