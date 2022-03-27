import json
import string

from bs4 import BeautifulSoup
import requests
from http.server import HTTPServer, BaseHTTPRequestHandler


# Некоторые функции


def gethtml(url) -> BeautifulSoup:
    HEADERS = {
        'User-Agent': 'Mozilla/5.0 (Windows NT 7.0; Win32; x32) AppleWebKit/537.36 (KHTML, like Gecko) '
                      'Chrome/74.0.3729.169 Safari/537.36'}
    html = requests.get(url, headers=HEADERS)
    return BeautifulSoup(html.text, "html.parser")


def getNews(page) -> string:
    # хранится запарсенные новости
    parsednews = []

    # Парсятся с сайта gkan.cap.ru
    url = f'https://gkan.cap.ru/news?type=news&page='+page
    soup = gethtml(url)
    news = soup.find_all("div", class_="item_news")

    for new in news:
        href = "https://gkan.cap.ru" + new.find("a")['href']
        lb = gethtml(href)
        img = lb.find("img", class_="map_img")['src']
        txt = lb.find("div", class_="news_text")

        parsednews.append({"date": new.find("span").text, "label": new.find("a").text, "url": href, "img": img,"text": txt.getText()})

    return json.dumps(parsednews)


class SimpleHTTPRequestHandler(BaseHTTPRequestHandler):

    def do_GET(self):
        url = self.path.split("/")
        print(self.path)
        if url[1] == "getnews":
            self.send_response(200)
            self.send_header('Content-type', 'application/json')
            self.end_headers()
            self.wfile.write(getNews(url[2]).encode())
        else:
            self.send_response(200)
            self.end_headers()
            self.wfile.write(b'Hello, world2!')


if __name__ == "__main__":
    httpd = HTTPServer(('', 80), SimpleHTTPRequestHandler)
    httpd.serve_forever()

