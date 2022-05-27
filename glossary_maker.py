import csv
import os
import copy
from googletrans import Translator
from httpcore import SyncHTTPProxy


class glossary:
    def __init__(self):
        self.keys = []
        self.english_words = []
        self.columns = []
        self.ROW_TITLE = 0
        self.ROW_LANG_CODE_ISO = 1
        self.ROW_LANG_CODE_GOOGLE = 2
        self.ROW_LANG_NAME = 3

    @staticmethod
    def XamlReturnToNormalReturn(string: str) -> str:
        return string.replace('&#13;&#10;', '\r\n')

    @staticmethod
    def NormalReturnToXamlReturn(string: str) -> str:
        return string.replace('\r\n', '&#13;&#10;')

    def load_csv(self, csv_file_name: str, encoding: str = 'utf-8'):
        lines = []
        with open(csv_file_name, encoding=encoding)as f:
            reader = csv.reader(f)
            for row in reader:
                lines.append(row)
        key_column_index = 0
        en_column_index = 1
        self.keys = [line[key_column_index] for line in lines]
        self.english_words = [line[en_column_index] for line in lines]
        for col in range(len(lines[0])):
            if col == key_column_index or col == en_column_index:
                continue
            self.columns.append([line[col] for line in lines])

    def save_csv(self, csv_file_name: str, encoding: str = 'utf-8'):
        lines = []
        for row in range(len(self.keys)):
            line = [column[row] for column in self.columns]
            lines.append([self.keys[row], self.english_words[row], *line])
        with open(csv_file_name, 'w', encoding=encoding, newline='') as f:
            writer = csv.writer(f)
            writer.writerows(lines)

    def do_translate(self, translator: Translator):
        for column in self.columns:
            for row in range(len(column)):
                if row <= self.ROW_LANG_NAME:
                    continue
                # 没有内容时才进行翻译
                if column[row] == '':
                    txt = translator.translate(self.XamlReturnToNormalReturn(self.english_words[row]), dest=column[self.ROW_LANG_CODE_GOOGLE]).text
                    column[row] = self.NormalReturnToXamlReturn(txt)
                else:
                    column[row] = ''

    def merge_columns(self, src):
        assert len(self.keys) == len(src.keys)
        assert len(self.columns) == len(src.columns)
        for col in range(len(self.columns)):
            for row in range(len(self.columns[col])):
                if self.columns[col][row] == '':
                    self.columns[col][row] = src.columns[col][row]

    def clear_content_rows(self):
        for column in self.columns:
            for row in range(len(column)):
                if row > self.ROW_LANG_NAME:
                    column[row] = ""

    def clone(self):
        return copy.deepcopy(self)


if __name__ == '__main__':

    CSV_FILE_NAME = 'glossary_for_PRemoteM.csv'
    CSV_FILE_NAME = os.path.basename(CSV_FILE_NAME).split('.')[0]

    g = glossary()
    g.load_csv(CSV_FILE_NAME + '.csv')

    http_proxy = SyncHTTPProxy((b'http', b'127.0.0.1', 1080, b''))
    proxies = {'http': http_proxy, 'https': http_proxy}
    translator = Translator(proxies=proxies)
    # print(translator.translate('hi\r\nsun rise', dest='de').text)

    # translate
    t = g.clone()
    t.do_translate(translator)
    t.save_csv(CSV_FILE_NAME + '_translated_part.csv')
    g.merge_columns(t)
    g.save_csv(CSV_FILE_NAME + '_translated_full.csv')
