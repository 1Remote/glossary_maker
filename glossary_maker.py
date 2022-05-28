import csv
import os
import copy
from googletrans import Translator
from httpcore import SyncHTTPProxy

Special_Characters_in_XAML_Keys = ['&', '<', '>', '\r', '\n']
Special_Characters_in_XAML_Values = ['&amp;', '&lt;', '&gt;', '&#13;', '&#10;']


def XamlReturnToNormalReturn(string: str) -> str:
    for i in range(len(Special_Characters_in_XAML_Keys)):
        string = string.replace(Special_Characters_in_XAML_Values[i], Special_Characters_in_XAML_Keys[i])
    return string


def NormalReturnToXamlReturn(string: str) -> str:
    for i in range(len(Special_Characters_in_XAML_Keys)):
        string = string.replace(Special_Characters_in_XAML_Keys[i], Special_Characters_in_XAML_Values[i])
    return string


class glossary:
    def __init__(self):
        self.keys = []
        self.english_words = []
        self.columns = []
        self.ROW_TITLE = 0
        self.ROW_LANG_CODE_ISO = 1
        self.ROW_LANG_CODE_GOOGLE = 2
        self.ROW_LANG_NAME = 3

    def load_csv(self, csv_file_name: str, encoding: str = 'utf-8'):
        lines = []  # [[key, english_word, a, b, c, d], [key1, w1, ...], ...]
        with open(csv_file_name, encoding=encoding)as f:
            reader = csv.reader(f)
            for row in reader:
                lines.append(row)
        key_column_index = 0
        en_column_index = 1
        self.keys = [line[key_column_index].replace('"', '') for line in lines]
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
                    txt = translator.translate(XamlReturnToNormalReturn(self.english_words[row]), dest=column[self.ROW_LANG_CODE_GOOGLE]).text
                    column[row] = NormalReturnToXamlReturn(txt)
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

    def save_to_xaml(self, encoding: str = 'utf-8'):
        langs = [self.english_words, *self.columns]
        for lang in langs:
            code = lang[self.ROW_LANG_CODE_ISO]
            name = lang[self.ROW_LANG_NAME]
            xaml_file_name = code + '.xaml'
            with open(xaml_file_name, 'w', encoding=encoding, newline='') as f:
                f.write('<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:s="clr-namespace:System;assembly=mscorlib" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">\r\n')
                f.write('    <s:String x:Key="language_name">' + name + '</s:String>\r\n')
                for row in range(len(self.keys)):
                    f.write('    <s:String x:Key="' + self.keys[row] + '">' + lang[row] + '</s:String>\r\n')
                f.write('</ResourceDictionary>')
            pass

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
    t.save_csv(CSV_FILE_NAME + '_translated_by_google.csv')
    g.merge_columns(t)
    g.save_csv(CSV_FILE_NAME + '_translated_full.csv')
    g.save_to_xaml()

    # save xaml
