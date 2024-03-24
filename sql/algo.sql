CREATE DATABASE algo /*!40100 COLLATE 'utf8_unicode_ci' */

CREATE TABLE accounts (
	id BIGINT(20) NOT NULL AUTO_INCREMENT,
	account TEXT NULL COMMENT 'Торговый счет' COLLATE 'utf8_unicode_ci',
	money DOUBLE NULL DEFAULT NULL COMMENT 'Размер счета',
	modifided_datetime DATETIME NULL DEFAULT NULL COMMENT 'Дата и время изменения',
	PRIMARY KEY (id)
)
COMMENT='Список счетов'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

CREATE TABLE logs (
	id BIGINT(20) NOT NULL AUTO_INCREMENT,
	datetime DATETIME NULL DEFAULT NULL,
	type TEXT NULL COLLATE 'utf8_unicode_ci',
	source TEXT NULL COLLATE 'utf8_unicode_ci',
	text TEXT NULL COLLATE 'utf8_unicode_ci',
	details TEXT NULL COLLATE 'utf8_unicode_ci',
	guid TEXT NULL COLLATE 'utf8_unicode_ci',
	PRIMARY KEY (id)
)
COMMENT='Логи'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
AUTO_INCREMENT=0
;

CREATE TABLE securities (
	id BIGINT(20) NOT NULL AUTO_INCREMENT,
	class_code TEXT NULL COMMENT 'Код класса' COLLATE 'utf8_unicode_ci',
	security_code TEXT NULL COMMENT 'Код инструмента' COLLATE 'utf8_unicode_ci',
	gaurant DOUBLE NULL DEFAULT NULL COMMENT 'Гарантийное обеспечение',
	modifided_datetime DATETIME NULL DEFAULT NULL COMMENT 'Дата и время изменения',
	PRIMARY KEY (id)
)
COMMENT='Список финансовых инструментов'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;


CREATE TABLE money_forts (
	id BIGINT(20) NOT NULL AUTO_INCREMENT,	
	date BIGINT(20) NULL DEFAULT NULL COMMENT 'Дата',
	time BIGINT(20) NULL DEFAULT NULL COMMENT 'Время',
	money DOUBLE NULL DEFAULT NULL COMMENT 'Состояние счета',
	PRIMARY KEY (id)
)
COMMENT='Состояние счета'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

CREATE TABLE strategies (
	id BIGINT(20) NOT NULL AUTO_INCREMENT,
	strategy_id BIGINT(20) NULL DEFAULT NULL COMMENT 'Идентификатор стратегии',
	strategy_name TEXT NULL COMMENT 'Наименование стратегии' COLLATE 'utf8_unicode_ci',
	account TEXT NULL COMMENT 'Торговый счет' COLLATE 'utf8_unicode_ci',
	firm_id TEXT NULL COMMENT 'Код фирмы' COLLATE 'utf8_unicode_ci',
	class_code TEXT NULL COMMENT 'Код класса' COLLATE 'utf8_unicode_ci',	
	security_code TEXT NULL COMMENT 'Код инструмента' COLLATE 'utf8_unicode_ci',
	lot_size BIGINT(20) NULL DEFAULT NULL COMMENT 'Размер лота',
	percent_money DOUBLE NULL DEFAULT NULL COMMENT 'Доля денег, выделенных стратегии',
	optimal_f DOUBLE NULL DEFAULT NULL COMMENT 'Оптимальное F',
	candles_db_table TEXT NULL COMMENT 'Имя таблицы в БД со свечами основного инструмента' COLLATE 'utf8_unicode_ci',
	candles_limit BIGINT(20) NULL DEFAULT NULL COMMENT 'Кол-во загружаемых свечей',
	order_quantity BIGINT(20) NULL DEFAULT NULL COMMENT 'Количество лотов для лимитной заявки',
	order_price DOUBLE NULL DEFAULT NULL COMMENT 'Цена для лимитной заявки',
	signal_name TEXT NULL COMMENT 'Имя сигнала' COLLATE 'utf8_unicode_ci',	
	current_position BIGINT(20) NULL DEFAULT NULL COMMENT 'Текущая позиция',
	net_profit DOUBLE NULL DEFAULT NULL COMMENT 'Прибыль убыток',
	current_net_profit DOUBLE NULL DEFAULT NULL COMMENT 'Прибыль убыток в текущей позиции',
	is_active BIGINT(20) NULL DEFAULT NULL COMMENT 'Флаг активности стратегии',
	modifided_datetime DATETIME NULL DEFAULT NULL COMMENT 'Дата и время изменения',	
	PRIMARY KEY (id)
)
COMMENT='Список стратегий'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
AUTO_INCREMENT=0
;

CREATE TABLE trades (
	id BIGINT(20) NOT NULL AUTO_INCREMENT,
	strategy_id BIGINT(20) NULL DEFAULT NULL COMMENT 'Идентификатор стратегии',
	trade_number BIGINT(20) NULL DEFAULT NULL COMMENT 'Номер сделки в торговой системе',
	order_number BIGINT(20) NULL DEFAULT NULL COMMENT 'Номер заявки в торговой системе',
	comment TEXT NULL COMMENT 'Комментарий' COLLATE 'utf8_unicode_ci',
	account TEXT NULL COMMENT 'Торговый счет' COLLATE 'utf8_unicode_ci',
	price DECIMAL(10,0) NULL DEFAULT NULL COMMENT 'Цена сделки',
	quantity BIGINT(20) NULL DEFAULT NULL COMMENT 'Количество бумаг в сделке в лотах',
	class_code TEXT NULL COMMENT 'Код класса' COLLATE 'utf8_unicode_ci',
	security_code TEXT NULL COMMENT 'Код инструмента' COLLATE 'utf8_unicode_ci',
	datetime DATETIME NULL DEFAULT NULL COMMENT 'Время совершения сделки',
	direction TEXT NULL COMMENT 'Направление сделки' COLLATE 'utf8_unicode_ci',
	modifided_datetime DATETIME NULL DEFAULT NULL COMMENT 'Дата и время добавления сделки',
	PRIMARY KEY (id)
)
COMMENT='Список сделок'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
AUTO_INCREMENT=0
;

CREATE TABLE candles_rts_m5 (
	id BIGINT(20) NOT NULL AUTO_INCREMENT,
	date BIGINT(20) NULL DEFAULT NULL COMMENT 'Дата свечи',
	time BIGINT(20) NULL DEFAULT NULL COMMENT 'Время свечи',
	open DOUBLE NULL DEFAULT NULL COMMENT 'Цена открытия',
	close DOUBLE NULL DEFAULT NULL COMMENT 'Цена закрытия',
	high DOUBLE NULL DEFAULT NULL COMMENT 'Максимальная цена',
	low DOUBLE NULL DEFAULT NULL COMMENT 'Минимальная цена',
	volume BIGINT(20) NULL DEFAULT NULL COMMENT 'Объем',
	timeframe BIGINT(20) NULL DEFAULT NULL COMMENT 'Таймфрейм в минутах',
	modifided_datetime DATETIME NULL DEFAULT NULL COMMENT 'Дата и время добавления свечи',
	PRIMARY KEY (id)
)
COMMENT='Свечи фьючерса RTS. Таймфрейм 5 минут'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

CREATE TABLE candles_sbrf_m5 (
	id BIGINT(20) NOT NULL AUTO_INCREMENT,
	date BIGINT(20) NULL DEFAULT NULL COMMENT 'Дата свечи',
	time BIGINT(20) NULL DEFAULT NULL COMMENT 'Время свечи',
	open DOUBLE NULL DEFAULT NULL COMMENT 'Цена открытия',
	close DOUBLE NULL DEFAULT NULL COMMENT 'Цена закрытия',
	high DOUBLE NULL DEFAULT NULL COMMENT 'Максимальная цена',
	low DOUBLE NULL DEFAULT NULL COMMENT 'Минимальная цена',
	volume BIGINT(20) NULL DEFAULT NULL COMMENT 'Объем',
	timeframe BIGINT(20) NULL DEFAULT NULL COMMENT 'Таймфрейм в минутах',
	modifided_datetime DATETIME NULL DEFAULT NULL COMMENT 'Дата и время добавления свечи',
	PRIMARY KEY (id)
)
COMMENT='Свечи фьючерса SBRF. Таймфрейм 5 минут'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

CREATE TABLE candles_si_m5 (
	id BIGINT(20) NOT NULL AUTO_INCREMENT,
	date BIGINT(20) NULL DEFAULT NULL COMMENT 'Дата свечи',
	time BIGINT(20) NULL DEFAULT NULL COMMENT 'Время свечи',
	open DOUBLE NULL DEFAULT NULL COMMENT 'Цена открытия',
	close DOUBLE NULL DEFAULT NULL COMMENT 'Цена закрытия',
	high DOUBLE NULL DEFAULT NULL COMMENT 'Максимальная цена',
	low DOUBLE NULL DEFAULT NULL COMMENT 'Минимальная цена',
	volume BIGINT(20) NULL DEFAULT NULL COMMENT 'Объем',
	timeframe BIGINT(20) NULL DEFAULT NULL COMMENT 'Таймфрейм в минутах',
	modifided_datetime DATETIME NULL DEFAULT NULL COMMENT 'Дата и время добавления свечи',
	PRIMARY KEY (id)
)
COMMENT='Свечи фьючерса Si. Таймфрейм 5 минут'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;