# Step to use

1. `pip install -r requirements.txt`
2. edit csv file e.g. `glossary_for_PRemoteM.csv`, add a new column with the name of the language code and name.
3. set proxy if needed in the `glossary_maker.py` `if __name__ == '__main__':`

    ```python
        http_proxy = SyncHTTPProxy((b'http', b'127.0.0.1', 1080, b''))
        proxies = {'http': http_proxy, 'https': http_proxy}
        translator = Translator(proxies=proxies)
    ```

4. run script `python glossary_maker.py` and it will create a new csv file with the translation result.